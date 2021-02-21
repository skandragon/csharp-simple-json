//
// https://github.com/skandragon/csharp-simple-json
//
//    Copyright 2021 Michael Graff
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using org.flame.SimpleJson;
using System;
using System.Collections.Generic;

public class MainClass {
        public static void Main(string[] args) {
            var x = new Example();
            x.Property1 = "string1";
            x.field1 = "field1string";
            x.Address = new PostalAddress("main st.", 12346);
            x.IntList = new List<int>() { 1, 2, 3 };
            x.Addresses = new List<PostalAddress>() {
                new PostalAddress("side st", 12345),
            };
            x.Dict = new Dictionary<string, int>() {
                { "this", 1 },
                { "that", 2 },
            };

            Console.WriteLine(SimpleJson.ToJson(x));
            Console.WriteLine(SimpleJson.ToJson(123));
            Console.WriteLine(SimpleJson.ToJson(123.456));
            Console.WriteLine(SimpleJson.ToJson("string"));
            Console.WriteLine(SimpleJson.ToJson("string\"\\"));
            Console.WriteLine(SimpleJson.ToJson("string\u0012"));
            Console.WriteLine(SimpleJson.ToJson(true));
            Console.WriteLine(SimpleJson.ToJson(false));
            Console.WriteLine(SimpleJson.ToJson(null));
            Console.WriteLine(SimpleJson.ToJson('c'));
            Console.WriteLine(SimpleJson.ToJson('\\'));
            Console.WriteLine(SimpleJson.ToJson('"'));
            Console.WriteLine(SimpleJson.ToJson('\u0012'));
        }
}

public class PostalAddress
{
    public string Street {get; set;}
    public int ZipCode {get; set;}

    public PostalAddress(string street, int zipcode) {
        this.Street = street;
        this.ZipCode = zipcode;
    }
}

public class Example
{
    [JsonName("prop1")]
    public string Property1 {get; set; }

    public int IntField {get; set; }

    public float FloatField {get; set;}

    [JsonIgnore]
    public int IgnoredField {get; set;}

    private int PrivateIntField {get; set;}

    public PostalAddress Address {get; set;}

    public string field1;

    public List<int> IntList {get; set;}

    public List<PostalAddress> Addresses {get; set;}
    
    public Dictionary<string, int> Dict {get; set;}
}


