using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait4
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncTest();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        private static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }


        public static void AsyncTest()
        {

            Console.WriteLine("AsyncTest() 方法开始执行，线程Id为：{0}", GetThreadId());

            Task task = Test1();

            Console.WriteLine("AsyncTest() 方法继续执行，线程Id为：{0}", GetThreadId());

            task.Wait();

            Console.WriteLine("AsyncTest() 方法结束执行，线程Id为：{0}", GetThreadId());

        }



        public static async Task Test1()
        {

            Console.WriteLine("Test1() 方法开始执行，线程Id为：{0}", GetThreadId());

            await Task.Factory.StartNew((state) =>
            {

                Console.WriteLine("Test1() 方法中的 {0} 开始执行，线程Id为：{1}", state, GetThreadId());

                Thread.Sleep(1000);

                Console.WriteLine("Test1() 方法中的 {0} 结束执行，线程Id为：{1}", state, GetThreadId());

            }, "task1");



            await  Task.Factory.StartNew((state) =>
            {

                Console.WriteLine("Test1() 方法中的 {0} 开始执行，线程Id为：{1}", state, GetThreadId());

                Thread.Sleep(3000);

                Console.WriteLine("Test1() 方法中的 {0} 结束执行，线程Id为：{1}", state, GetThreadId());

            }, "task2");



            Console.WriteLine("Test1() 方法结束执行，线程Id为：{0}", GetThreadId());

        }
    }
}
