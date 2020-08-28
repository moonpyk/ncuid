ncuid [![Build status](https://ci.appveyor.com/api/projects/status/s1e5w82tc8k6d9cg)](https://ci.appveyor.com/project/moonpyk/ncuid)
=====

Collision-resistant ids for .NET (port of https://github.com/ericelliott/cuid)

## Nuget availability

    PM> Install-Package NCuid

## Example

    using System;
    using NCuid;

    namespace CUIDTest
    {
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello World! This is a Cuid -> " + Cuid.Generate() + " <-" );
            }
        }
    }
