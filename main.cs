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

            Console.WriteLine(SkandragonSimpleJson.ToJson(x));
            Console.WriteLine(SkandragonSimpleJson.ToJson(123));
            Console.WriteLine(SkandragonSimpleJson.ToJson(123.456));
            Console.WriteLine(SkandragonSimpleJson.ToJson("string"));
            Console.WriteLine(SkandragonSimpleJson.ToJson("string\"\\"));
            Console.WriteLine(SkandragonSimpleJson.ToJson("string\u0012"));
            Console.WriteLine(SkandragonSimpleJson.ToJson(true));
            Console.WriteLine(SkandragonSimpleJson.ToJson(false));
            Console.WriteLine(SkandragonSimpleJson.ToJson(null));
            Console.WriteLine(SkandragonSimpleJson.ToJson('c'));
            Console.WriteLine(SkandragonSimpleJson.ToJson('\\'));
            Console.WriteLine(SkandragonSimpleJson.ToJson('"'));
            Console.WriteLine(SkandragonSimpleJson.ToJson('\u0012'));
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
    [SkandragonJsonName("prop1")]
    public string Property1 {get; set; }

    public int IntField {get; set; }

    public float FloatField {get; set;}

    [SkandragonJsonIgnore]
    public int IgnoredField {get; set;}

    private int PrivateIntField {get; set;}

    public PostalAddress Address {get; set;}

    public string field1;

    public List<int> IntList {get; set;}

    public List<PostalAddress> Addresses {get; set;}
}


