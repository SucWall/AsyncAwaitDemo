using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait3
{
    class Program
    {
        /// <summary>
        /// 3. 模拟去买盐（不止关心是否执行完成，还要获取执行结果。返回 Task<TResult> 类型）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CookDinner();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }


        /// <summary>
        /// 做饭
        /// </summary>
        public static void CookDinner()
        {

            Console.WriteLine("老婆开始做饭，线程Id为：{0}", GetThreadId());

            Console.WriteLine("哎呀，没盐了");
            Stopwatch sw = new Stopwatch();
            sw.Start();//开始计时
            Task<string> task = CommandBuySalt();
            sw.Stop();//结束计时
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("不管他继续炒菜，线程Id为：{0}", GetThreadId());

            Thread.Sleep(100);

           // string result = task.Result;    //必须要用盐了，等我把盐回来（停止炒菜（阻塞线程））

           // Console.WriteLine("用了盐炒的菜就是好吃【{0}】，线程Id为：{1}", result, GetThreadId());

            Console.WriteLine("老婆把饭做好了，线程Id为：{0}", GetThreadId());

        }



        /// <summary>
        /// 通知我去买盐
        /// </summary>
        public static async Task<string> CommandBuySalt()
        {
            Console.WriteLine("这时我准备去买盐了，线程Id为：{0}", GetThreadId());

            //string result = await Task.Run(() =>
            //{

            //    Console.WriteLine("屁颠屁颠的去买盐，线程Id为：{0}", GetThreadId());

            //    Thread.Sleep(1000);

            //    return "盐买回来了，顺便我还买了一包烟";

            //});
             var result=  await  CommandBuySalt2();
            Console.WriteLine("线程Id为：{0}", GetThreadId());

            var result2 = await CommandBuySalt2();

            Console.WriteLine("{0}，线程Id为：{1}", result, GetThreadId());
            Console.WriteLine("{0}，线程Id为：{1}", result2, GetThreadId());
            return result;
        }


        /// <summary>
        /// 通知我去买盐
        /// </summary>
        public static async Task<string> CommandBuySalt2()
        {
            Console.WriteLine("CommandBuySalt2：{0}", GetThreadId());
            await Task.Run(() =>
            {
                Console.WriteLine("屁颠屁颠的去买盐，线程Id为：{0}", GetThreadId());

                Thread.Sleep(1000);


            });
            Console.WriteLine("盐买回来了，顺便我还买了一包烟：{0}", GetThreadId());
            return "盐买回来了，顺便我还买了一包烟";
        }
    }
}
