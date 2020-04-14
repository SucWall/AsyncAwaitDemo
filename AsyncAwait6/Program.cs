using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait6
{
    class Program
    {
        static void Main(string[] args)
        {
            CookDinner_MultiCancelBuySalt();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
        /// <summary>
        /// 做饭（多个消息传达买盐任务取消）
        /// </summary>
        public static void CookDinner_MultiCancelBuySalt()
        {
            Console.WriteLine("老婆开始做饭，线程Id为：{0}", GetThreadId());
            Console.WriteLine("哎呀，没盐了");
            CancellationTokenSource source1 = new CancellationTokenSource();    //因为存在而取消
            CancellationTokenSource source2 = new CancellationTokenSource();    //因为放弃而取消

            CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(source1.Token, source2.Token);

            //注册取消时的回调委托
            source1.Token.Register(() =>
            {
                Console.WriteLine("这是因为{0}所以取消，线程Id为：{1}", "家里还有盐", GetThreadId());
            });

            source2.Token.Register((state) =>
            {
                Console.WriteLine("这是因为{0}所以取消，线程Id为：{1}", state, GetThreadId());
            }, "不做了出去吃");

            source.Token.Register((state) =>
            {
                Console.WriteLine("这是因为{0}所以取消，线程Id为：{1}", state, GetThreadId());
            }, "没理由");

            //这里必须传递 CancellationTokenSource.CreateLinkedTokenSource() 方法返回的 Token 对象
            Task<string> task = CommandBuySalt_MultiCancelBuySalt(source.Token);

            Console.WriteLine("等等，好像不用买了，线程Id为：{0}", GetThreadId());
            Thread.Sleep(100);

            string[] results = new string[] { "家里的盐", "不做了出去吃", "没理由" };
            Random r = new Random();
            switch (r.Next(1, 4))
            {
                case 1:
                    source1.Cancel();           //传达取消请求（家里有盐）
                                                //source1.CancelAfter(3000);  //3s后才调用取消的回调方法
                    Console.WriteLine("既然有盐我就继续炒菜【{0}】，线程Id为：{1}", results[0], GetThreadId());
                    break;
                case 2:
                    source2.Cancel();           //传达取消请求（不做了出去吃）
                                                //source2.CancelAfter(3000);  //3s后才调用取消的回调方法
                    Console.WriteLine("我们出去吃不用买啦【{0}】，线程Id为：{1}", results[1], GetThreadId());
                    break;
                case 3:
                    source.Cancel();            //传达取消请求（没理由）
                                                //source.CancelAfter(3000);   //3s后才调用取消的回调方法
                    Console.WriteLine("没理由就是不用买啦【{0}】，线程Id为：{1}", results[2], GetThreadId());
                    break;
            }

            Console.WriteLine("最终的任务状态是：{0}，已完成：{1}，已取消：{2}，已失败：{3}",
                task.Status, task.IsCompleted, task.IsCanceled, task.IsFaulted);
        }

        private static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// 通知我去买盐（又告诉我不用买了，各种理由）
        /// </summary>
        public static async Task<string> CommandBuySalt_MultiCancelBuySalt(CancellationToken token)
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
    }
}
