using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait5
{
    class Program
    {
        static void Main(string[] args)
        {
            CookDinner_CancelBuySalt();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }


        /// <summary>
        /// 做饭（买盐任务取消）
        /// </summary>
        public static void CookDinner_CancelBuySalt()
        {
            Console.WriteLine("老婆开始做饭，线程Id为：{0}", GetThreadId());
            Console.WriteLine("哎呀，没盐了");
            CancellationTokenSource source = new CancellationTokenSource();
            Task<string> task =CommandBuySalt_CancelBuySalt(source.Token);
            Console.WriteLine("不管他继续炒菜，线程Id为：{0}", GetThreadId());
            Thread.Sleep(100);

            string result = "家里的盐";
            if (!string.IsNullOrEmpty(result))
            {
                source.Cancel();    //传达取消请求
                Console.WriteLine("家里还有盐不用买啦，线程Id为：{0}", GetThreadId());
            }
            else
            {
                //如果已取消就不能再获得结果了（否则将抛出 System.Threading.Tasks.TaskCanceledException 异常）
                //你都叫我不要买了，我拿什么给你？
                result = task.Result;
            }

            Console.WriteLine("既然有盐我就继续炒菜【{0}】，线程Id为：{1}", result, GetThreadId());
            Console.WriteLine("老婆把饭做好了，线程Id为：{0}", GetThreadId());
            Console.WriteLine("最终的任务状态是：{0}，已完成：{1}，已取消：{2}，已失败：{3}",
                task.Status, task.IsCompleted, task.IsCanceled, task.IsFaulted);
        }

        /// <summary>
        /// 通知我去买盐（又告诉我不用买了）
        /// </summary>
        public static  async Task<string> CommandBuySalt_CancelBuySalt(CancellationToken token)
        {
            Console.WriteLine("这时我准备去买盐了，线程Id为：{0}", GetThreadId());

            //已开始执行的任务不能被取消
            string result = await Task.Run(() =>
            {
                Console.WriteLine("屁颠屁颠的去买盐，线程Id为：{0}", GetThreadId());
                Thread.Sleep(1000);
            }, token).ContinueWith((t) =>  //若没有取消就继续执行
            {
                Console.WriteLine("盐已经买好了，线程Id为：{0}", GetThreadId());
                Thread.Sleep(1000);

                return "盐买回来了，顺便我还买了一包烟";
            }, token);

            Console.WriteLine("{0}，线程Id为：{1}", result, GetThreadId());

            return result;
        }

        private static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }
    }
}
