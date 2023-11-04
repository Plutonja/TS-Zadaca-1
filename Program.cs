using System;

class Program
{
    static void Main(string[] args)
    {
        Stan[] stanovi = new Stan[4];
        stanovi[0] = new NenamjestenStan(50, Lokacija.Gradsko, true);
        stanovi[1] = new NenamjestenStan(80, Lokacija.Prigradsko, true);
        stanovi[2] = new NamjestenStan(40, Lokacija.Prigradsko, true, 2000, 2);
        stanovi[3] = new NamjestenStan(80, Lokacija.Gradsko, false, 3000, 6);

        Console.WriteLine("Površina Lokacija Namješten Internet Vrijednost namještaja Broj aparata");
        foreach (Stan stan in stanovi)
        {
            stan.Ispisi();
        }

        int minPovrsina = 0;
        int maxPovrsina = 0;

        Console.WriteLine("Unesite minimalnu zeljenu povrsinu");
        while (!Int32.TryParse(Console.ReadLine(), out minPovrsina) || minPovrsina < 0)
        {
            Console.WriteLine("Unos nije ispravan");
        }

        Console.WriteLine("Unesite maksimalnu zeljenu povrsinu");
        while (!Int32.TryParse(Console.ReadLine(), out maxPovrsina) || maxPovrsina < 0)
        {
            Console.WriteLine("Unos nije ispravan");
        }

        foreach (Stan stan in stanovi)
        {
            if (stan.BrojKvadrata >= minPovrsina && stan.BrojKvadrata <= maxPovrsina)
            {
                stan.Ispisi();
                Console.WriteLine("Ukupna cijena najma stana je {0:F2}.", stan.ObracunajCijenuNajma());
            }
        }

        Console.ReadLine();
    }
}

enum Lokacija
{
    Gradsko,
    Prigradsko
}

abstract class Stan
{
    public int BrojKvadrata { get; }
    public Lokacija Lokacija { get; }
    public bool Internet { get; }

    public Stan(int brojKvadrata, Lokacija lokacija, bool internet)
    {
        BrojKvadrata = brojKvadrata;
        Lokacija = lokacija;
        Internet = internet;
    }

    public abstract void Ispisi();
    public abstract decimal ObracunajCijenuNajma();
}

class NenamjestenStan : Stan
{
    public NenamjestenStan(int brojKvadrata, Lokacija lokacija, bool internet)
        : base(brojKvadrata, lokacija, internet)
    {
    }

    public override void Ispisi()
    {
        Console.WriteLine($"{BrojKvadrata} {Lokacija} Nenamješten {Internet}");
    }

    public override decimal ObracunajCijenuNajma()
    {
        decimal osnovnaCijena = Lokacija == Lokacija.Gradsko ? 200M : 150M;
        decimal cijenaKvadrata = BrojKvadrata;
        decimal cijena = osnovnaCijena + cijenaKvadrata;

        if (Internet)
        {
            cijena += cijena * 0.02M;
        }

        return cijena;
    }
}

class NamjestenStan : Stan
{
    public decimal VrijednostNamjestaja { get; }
    public int BrojAparata { get; }

    public NamjestenStan(int brojKvadrata, Lokacija lokacija, bool internet, decimal vrijednostNamjestaja, int brojAparata)
        : base(brojKvadrata, lokacija, internet)
    {
        VrijednostNamjestaja = vrijednostNamjestaja;
        BrojAparata = brojAparata;
    }

    public override void Ispisi()
    {
        Console.WriteLine($"{BrojKvadrata} {Lokacija} Namješten {Internet} {VrijednostNamjestaja} {BrojAparata}");
    }

    public override decimal ObracunajCijenuNajma()
    {
        decimal osnovnaCijena = Lokacija == Lokacija.Gradsko ? 200M : 150M;
        decimal cijenaKvadrata = BrojKvadrata;
        decimal cijena = osnovnaCijena + cijenaKvadrata;

        if (Internet)
        {
            cijena += cijena * 0.02M;
        }

        decimal namjestajPovecanje = 0;
        if (BrojAparata < 3)
        {
            namjestajPovecanje = VrijednostNamjestaja * 0.01M;
        }
        else
        {
            namjestajPovecanje = VrijednostNamjestaja * 0.02M;
        }

        return cijena + namjestajPovecanje;
    }
}
