using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait2
{
    class Program
    {
        /// <summary>
        ///  2.模拟打开电源开关（关心是否执行完成，返回 Task 类型）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            OpenMainsSwitch();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }



        /// <summary>
        /// 打开电源开关
        /// </summary>
        public static void OpenMainsSwitch()
        {

            Console.WriteLine("我和老婆正在看电视，线程Id为：{0}", GetThreadId());

            Console.WriteLine("突然停电了，快去看下是不是跳闸了");

            Task task = CommandOpenMainsSwitch();

            Console.WriteLine("没电了先玩会儿手机吧，线程Id为：{0}", GetThreadId());

            Thread.Sleep(100);

            Console.WriteLine("手机也没电了只等电源打开，线程Id为：{0}", GetThreadId());

            while (!task.IsCompleted) { Thread.Sleep(100); }

            Console.WriteLine("又有电了我们继续看电视，线程Id为：{0}", GetThreadId());
        }



        /// <summary>
        /// 通知我去打开电源开关
        /// </summary>

        public static async Task CommandOpenMainsSwitch()
        {
            Console.WriteLine("这时我准备去打开电源开关，线程Id为：{0}", GetThreadId());

            await Task.Run(() =>
            {
                Console.WriteLine("屁颠屁颠的去打开电源开关，线程Id为：{0}", GetThreadId());

                Thread.Sleep(1000);
            });

            Console.WriteLine("电源开关打开了，线程Id为：{0}", GetThreadId());
        }
    }
}
