
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

namespace XxDefinitions
{
	public interface INetPacketTree
	{
		/// <summary>
		/// 获取到该节点的Packet
		/// </summary>
		/// <returns></returns>
		ModPacket GetPacket();
		/// <summary>
		/// 到该节点时执行的函数
		/// </summary>
		/// <param name="reader">Packet</param>
		/// <param name="whoAmI">发出Packet的玩家</param>
		void Handle(BinaryReader reader,int whoAmI);
	}
	/// <summary>
	/// 作为Packet操作的后续结点接口
	/// </summary>
	/// <typeparam name="FatherType"></typeparam>
	public interface INetPacketTreeChild<FatherType>: INetPacketTree {
		/// <summary>
		/// 该节点的父亲
		/// </summary>
		NetPacketTreeFather<FatherType> Father { get; set; }
		/// <summary>
		/// 自己在父亲中的Key
		/// </summary>
		FatherType childKey { get; set; }
		
	}
	
	/// <summary>
	/// 作为Packet操作的前驱结点类，支持任意标识ChildType
	/// 设置SetBinary，GetBinary来设置从ChildType输入输出Binary的方法
	/// 用AddChild加入子节点，会自动设置Binary来传给对应的Child
	/// </summary>
	public abstract class NetPacketTreeFather<ChildType> : INetPacketTree {
		public delegate void DWriteBinary(BinaryWriter writer, ChildType data);
		/// <summary>
		/// 用于从data输入Binary
		/// </summary>
		public DWriteBinary WriteBinary;
		public delegate ChildType DReadBinary(BinaryReader reader);
		/// <summary>
		/// 用于从Binary得到data
		/// </summary>
		public DReadBinary ReadBinary;
		/*
		public ChildType Lastkay;
		public delegate ChildType DNextAvaliableF(ChildType lastkey, IList<ChildType> NetPacketTreeChildsKayList);
		/// <summary>
		/// NextAvaliableF用于获取下一个可用的key
		/// 注意第一次使用时Lastkay=default(ChildType)
		/// </summary>
		public DNextAvaliableF NextAvaliableF;
		public ChildType NextAvaliable() {
			return (Lastkay=NextAvaliableF(Lastkay, NetPacketTreeChilds.Keys));
		}
		*/
		/// <summary>
		/// 存储子节点的表
		/// </summary>
		public SortedList<ChildType, WeakReference< INetPacketTreeChild<ChildType>> > NetPacketTreeChilds = new SortedList<ChildType, WeakReference<INetPacketTreeChild<ChildType>> >();
		/// <summary>
		/// 获取用于子节点的Packet
		/// </summary>
		/// <param name="childKey">子节点的Key</param>
		/// <returns></returns>
		public ModPacket GetPacketChild(ChildType childKey) {
			ModPacket mp = GetPacket();
			WriteBinary(mp, childKey);
			return mp;
		}
		public abstract ModPacket GetPacket();
		/// <summary>
		/// 传输Packet到对应Key的Child
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="whoAmI"></param>
		public void Handle(BinaryReader reader, int whoAmI) {
			INetPacketTreeChild < ChildType > child;
			ChildType key= ReadBinary(reader);
			NetPacketTreeChilds[key].TryGetTarget(out child);
			if (child != null) child.Handle(reader, whoAmI);
			else NetPacketTreeChilds.Remove(key);
		}
		/// <summary>
		/// 新增Child
		/// </summary>
		/// <param name="Child">子节点</param>
		/// <param name="childKey">子节点的Key</param>
		public void AddChild(INetPacketTreeChild<ChildType> Child, ChildType childKey) {
			NetPacketTreeChilds.Add(childKey, new WeakReference<INetPacketTreeChild<ChildType> >(Child));
			Child.childKey = childKey;
			Child.Father = this;
		}
		public void RemoveChild( ChildType childKey)
		{
			NetPacketTreeChilds.Remove(childKey);
		}
		public NetPacketTreeFather(DWriteBinary WriteBinary, DReadBinary ReadBinary) {
			this.WriteBinary = WriteBinary;
			this.ReadBinary = ReadBinary;
		}
	}
	/// <summary>
	/// 进行Packet操作的根节点
	/// </summary>
	/// <typeparam name="ChildType"></typeparam>
	public class NetPacketTreeMain<ChildType> : NetPacketTreeFather<ChildType> {
		/// <summary>
		/// 该mod
		/// </summary>
		public Mod mod;
		public override ModPacket GetPacket() { return mod.GetPacket(); }
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="mod">该mod</param>
		/// <param name="SetBinary">SetBinary方法</param>
		/// <param name="GetBinary">GetBinary方法</param>
		public NetPacketTreeMain(Mod mod, DWriteBinary SetBinary, DReadBinary GetBinary) : base(SetBinary, GetBinary) {
			this.mod = mod;
			XxDefinitions.Logv1.Debug($"NetPacketTreeMain {this.GetType().FullName} ctor");
		}
		~NetPacketTreeMain() {
			XxDefinitions.Logv1.Debug($"NetPacketTreeMain {this.GetType().FullName} ~");
		}
	}
	/// <summary>
	/// 进行Packet操作的分支结点
	/// </summary>
	/// <typeparam name="FatherType"></typeparam>
	/// <typeparam name="ChildType"></typeparam>
	public class NetPacketTreeNode<FatherType, ChildType> : NetPacketTreeFather<ChildType>, INetPacketTreeChild<FatherType> {
		public FatherType _childKey;
		public FatherType childKey
		{
			get { return _childKey; }
			set { _childKey = value; }
		}
		public WeakReference<NetPacketTreeFather<FatherType>> _Father;
		public NetPacketTreeFather<FatherType> Father
		{
			get {
				if (_Father.TryGetTarget(out var t))
					return t;
				else return null;
			}
			set { _Father.SetTarget(value); }
		}
		public override ModPacket GetPacket()
		{
			return Father.GetPacketChild(childKey);
		}
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="Father">父亲</param>
		/// <param name="childKey">该节点的Key</param>
		/// <param name="WriteBinary">SetBinary方法</param>
		/// <param name="ReadBinary">GetBinary方法</param>
		public NetPacketTreeNode(NetPacketTreeFather<FatherType> Father, FatherType childKey, DWriteBinary WriteBinary, DReadBinary ReadBinary) : base(WriteBinary, ReadBinary)
		{
			_Father = new WeakReference<NetPacketTreeFather<FatherType>>(Father);
			this.Father.AddChild(this, childKey);
			this.childKey = childKey;
			XxDefinitions.Logv1.Debug($"NetPacketTreeNode {this.GetType().FullName} ctor");
		}
		~NetPacketTreeNode()
		{
			if (Father != null)
				Father.NetPacketTreeChilds.Remove(childKey);
			XxDefinitions.Logv1.Debug($"NetPacketTreeNode {this.GetType().FullName} ~");
		}
	}
	/// <summary>
	/// 进行Packet操作的叶子结点，进行操作
	/// </summary>
	/// <typeparam name="FatherType"></typeparam>
	public class NetPacketTreeLeaf<FatherType> : INetPacketTreeChild<FatherType> {
		public FatherType _childKey;
		public FatherType childKey
		{
			get { return _childKey; }
			set { _childKey = value; }
		}
		public WeakReference<NetPacketTreeFather<FatherType>> _Father;
		public NetPacketTreeFather<FatherType> Father
		{
			get
			{
				if (_Father.TryGetTarget(out var t))
					return t;
				else return null;
			}
			set { _Father.SetTarget(value); }
		}
		public delegate void DHandleFunction(BinaryReader reader, int whoAmI);
		/// <summary>
		/// 进行操作的函数
		/// </summary>
		public DHandleFunction HandleFunction;
		public void Handle(BinaryReader reader, int whoAmI) {
			HandleFunction(reader, whoAmI);
		}
		public ModPacket GetPacket() {
			return Father.GetPacketChild(childKey);
		}
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="HandleFunction">进行操作的函数</param>
		/// <param name="childKey"></param>
		public NetPacketTreeLeaf(DHandleFunction HandleFunction, NetPacketTreeFather<FatherType> Father, FatherType childKey, Action<ModPacket> AutoDoFunc=null)
		{
			_Father = new WeakReference<NetPacketTreeFather<FatherType>>(Father);
			this.HandleFunction = HandleFunction;
			this.Father.AddChild(this, childKey);
			this.childKey = childKey;
			this.AutoDoFunc = AutoDoFunc;
			XxDefinitions.Logv1.Debug($"NetPacketTreeNode {this.GetType().FullName} ctor");
		}
		~NetPacketTreeLeaf(){
			if(Father!=null)
				Father.NetPacketTreeChilds.Remove(childKey);
			//Main.NewText("Leaf released");
			XxDefinitions.Logv1.Debug($"NetPacketTreeLeaf {this.GetType().FullName} ~");
		}
		/// <summary>
		/// 用AutoDoFunc自动完成并发送
		/// </summary>
		public void AutoDo(int toClient = -1, int ignoreClient = -1) {
			ModPacket mp = GetPacket();
			AutoDoFunc(mp);
			mp.Send(toClient, ignoreClient);
		}
		/// <summary>
		/// 用于自动完成ModPacket的函数
		/// </summary>
		public Action<ModPacket> AutoDoFunc;
	}
	public static class BinaryIOFunc {
		public static void WriteBinaryInt(BinaryWriter writer, int data) { writer.Write(data); }
		public static int ReadBinaryInt(BinaryReader reader) { return reader.ReadInt32(); }
		public static void WriteBinaryString(BinaryWriter writer, string data) { writer.Write(data); }
		public static string ReadBinaryString(BinaryReader reader) { return reader.ReadString(); }
	}
}
