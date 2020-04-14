using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait1
{
    class Program
    {
        /// <summary>
        /// 模拟扔垃圾（不关心结果，返回 void 类型）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DropLitter();
            Console.ReadKey();
        }



        /// <summary>
        /// 扔垃圾
        /// </summary>
        public static void DropLitter()
        {

            Console.WriteLine("老婆开始打扫房间，线程Id为：{0}", GetThreadId());

            Console.WriteLine("垃圾满了，快去扔垃圾");

            CommandDropLitter();

            Console.WriteLine("不管他继续打扫，线程Id为：{0}", GetThreadId());

            Thread.Sleep(100);

            Console.WriteLine("老婆把房间打扫好了，线程Id为：{0}", GetThreadId());

        }

        private static int GetThreadId()
        {
          return  Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>

        /// 通知我去扔垃圾

        /// </summary>

        public static async void CommandDropLitter()
        {

            Console.WriteLine("这时我准备去扔垃圾，线程Id为：{0}", GetThreadId());

            await Task.Run(() =>
            {

                Console.WriteLine("屁颠屁颠的去扔垃圾，线程Id为：{0}", GetThreadId());

                Thread.Sleep(1000);

            });
            Console.WriteLine("垃圾扔了还有啥吩咐，线程Id为：{0}", GetThreadId());

        }
    }
}
