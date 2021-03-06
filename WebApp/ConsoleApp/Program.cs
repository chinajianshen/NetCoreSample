﻿using Foundatio.Messaging;
using Foundatio.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //MessageBusTest();

            BinaryTreeArithmetic();
         
            //Console.WriteLine(GetFileContent());

            Console.ReadKey();
        }

        static void BinaryTreeArithmetic()
        {
            var tree = new NodeTree()
            {
                Value = "1",
                ChildTree = new List<NodeTree>() {
                new NodeTree(){ Value="1-1",
                    ChildTree =new List<NodeTree>(){
                        new NodeTree() { Value="1-1-1"},
                        new NodeTree() { Value="1-1-2"}
                    }
                },
                new NodeTree(){ Value="1-2",
                    ChildTree =new List<NodeTree>(){
                        new NodeTree() { Value="1-2-1"},
                        new NodeTree() { Value="1-2-2",
                            ChildTree=new List<NodeTree>(){
                                new NodeTree() { Value="1-2-2-1"}
                            }
                        }
                    }
                },
                new NodeTree(){ Value="1-3"}
            }
            };
            var node = SearchNode(tree, "1-3");
            Console.WriteLine(node.Value);

            Console.WriteLine("===================");

            node = SearchNode2(tree, "1-2-1");
            Console.WriteLine(node.Value);
        }

        //深度优先，递归
        static NodeTree SearchNode(NodeTree tree, string valueToFind)
        {
            if (tree.Value == valueToFind)
            {
                return tree;
            }
            else
            {
                if (tree.ChildTree != null)
                {
                    foreach (var item in tree.ChildTree)
                    {
                        var temp = SearchNode(item, valueToFind);
                        if (temp != null) return temp;
                    }
                }
            }
            return null;
        }

        //堆栈
        static NodeTree SearchNode2(NodeTree rootNode, string valueToFind)
        {
            var stack = new Stack<NodeTree>(new[] { rootNode });
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n.Value == valueToFind) return n;
                if (n.ChildTree != null)
                    foreach (var child in n.ChildTree) stack.Push(child);
            }
            return null;
        }

        private static async Task<string> GetFileContent()
        {
            IFileStorage storage = new InMemoryFileStorage();
            await storage.SaveFileAsync("test.txt","123456");
            string content = await storage.GetFileContentsAsync("test.txt");
            return content;
        }

        private static async void MessageBusTest()
        {
            IMessageBus messageBus = new InMemoryMessageBus();
            messageBus.SubscribeAsync<SimpleMessageA>(msg =>
            {
                Console.WriteLine(msg.Data);
            });
            await messageBus.PublishAsync(new SimpleMessageA { Data= "hello" });
        }
    }

    public class SimpleMessageA
    {
        public string Data { get; set; }
    }

    public class NodeTree
    {
        public string Value { get; set; }

        public List<NodeTree> ChildTree { get; set; }
    }
}
