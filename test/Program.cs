// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

static int[] reverse(int[] p)
{

    for (int i = 0; i <= p.Length/2; i++)
    {
        var f = p[i];
        var l = p[p.Length-1-i];

        p[i] = l; 
        p[p.Length - 1 - i] = f;
    }

    return p;
}