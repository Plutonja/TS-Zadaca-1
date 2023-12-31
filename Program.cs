﻿using System;

class Program
{
    static void Main(string[] args)
    {
        Agencija agencija = new Agencija();

        // Dodajte stanove
        agencija.DodajStan(new NenamjestenStan(50, Lokacija.Gradsko, true));
        agencija.DodajStan(new NenamjestenStan(80, Lokacija.Prigradsko, true));
        agencija.DodajStan(new NamjestenStan(40, Lokacija.Prigradsko, true, 2000, 2));
        agencija.DodajStan(new NamjestenStan(80, Lokacija.Gradsko, false, 3000, 6));
        agencija.DodajStan(new LuksuzniApartman(350, Lokacija.Gradsko, true, 10000, 12, agencija.DohvatiOsoblje()));

        // Dodavanje osoblja
        agencija.DodajOsoblje(new Batler("Alfred", "Pennyworth", new DateTime(2018, 7, 10), 3000, 10));
        agencija.DohvatiOsoblje().OfType<Batler>().FirstOrDefault()?.PostaviGodineIskustva(40);

        agencija.DodajOsoblje(new Kuhar("Gordon", "Ramsay", new DateTime(2016, 4, 1), 2500));
        agencija.DohvatiOsoblje().OfType<Kuhar>().FirstOrDefault()?.DodajJelo("Cevapi");
        agencija.DohvatiOsoblje().OfType<Kuhar>().FirstOrDefault()?.DodajJelo("Pita ispod saca");
        
        agencija.DodajOsoblje(new Vrtlar("Najur", "Konig", new DateTime(2001, 11, 12), 4000, 50, true));
        
        // Ispis svih stanova sortiranih po cijeni
        Console.WriteLine("\nSvi stanovi sortirani po cijeni:");
        agencija.IspisiSveStanoveSortiranePoCijeni();

        // Ispisivanje osoblja sortiranog po plati
        Console.WriteLine("\nOsoblje sortirano po plati:");
        agencija.IspisiSveOsobljeSortiranoPoPlati();

        // Dodavanje novih stanova
        string dodajStanOdgovor;
        do
        {
            Console.WriteLine("Želite li dodati novi stan? (da/ne)");
            dodajStanOdgovor = Console.ReadLine().ToLower();

            if (dodajStanOdgovor == "da")
            {
                // Unos informacija o novom stanu
                Console.WriteLine("Unesite informacije o novom stanu:");

                Console.WriteLine("Unesite površinu stana:");
                int povrsina = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Unesite lokaciju stana (Gradsko/Prigradsko):");
                Lokacija lokacija = (Lokacija)Enum.Parse(typeof(Lokacija), Console.ReadLine(), true);

                Console.WriteLine("Stan ima internet? (da/ne):");
                bool imaInternet = Console.ReadLine().ToLower() == "da";

                Console.WriteLine("Je li stan namješten? (da/ne):");
                bool jeNamjesten = Console.ReadLine().ToLower() == "da";

                // Dodatne informacije
                decimal cijenaNamjestaja = 0;
                int brojAparata = 0;

                if (jeNamjesten)
                {
                    Console.WriteLine("Unesite cijenu namještaja:");
                    decimal.TryParse(Console.ReadLine(), out cijenaNamjestaja);

                    Console.WriteLine("Unesite broj aparata:");
                    int.TryParse(Console.ReadLine(), out brojAparata);
                }

                // Dodavanje novog stana
                if (jeNamjesten)
                {
                    agencija.DodajStan(new NamjestenStan(povrsina, lokacija, imaInternet, cijenaNamjestaja, brojAparata));
                }
                else
                {
                    agencija.DodajStan(new NenamjestenStan(povrsina, lokacija, imaInternet));
                }

                Console.WriteLine("Stan dodan!");
            }
        } while (dodajStanOdgovor == "da");

        // Dodavanje osoblja
        string dodajOsobljeOdgovor;
        do
        {
            Console.WriteLine("Želite li dodati novo osoblje? (da/ne)");
            dodajOsobljeOdgovor = Console.ReadLine().ToLower();

            if (dodajOsobljeOdgovor == "da")
            {
                // Unos informacija o novom osoblju
                Console.WriteLine("Unesite informacije o novom osoblju:");

                Console.WriteLine("Unesite ime osobe:");
                string ime = Console.ReadLine();

                Console.WriteLine("Unesite prezime osobe:");
                string prezime = Console.ReadLine();

                Console.WriteLine("Unesite vrstu posla (Batler/Vrtlar/Kuhar):");
                string vrstaPosla = Console.ReadLine();

                // Dodatne informacije
                decimal plata = 0;
                Console.WriteLine("Unesite mjesečnu platu:");
                decimal.TryParse(Console.ReadLine(), out plata);

                // Inicijalizacija varijable izvan switch bloka
                Osoba novoOsoblje = null;

                switch (vrstaPosla.ToLower())
                {
                    case "batler":
                        // Unos godina iskustva
                        Console.WriteLine("Unesite godine iskustva za batlera:");
                        int godineIskustvaBatlera;
                        while (!int.TryParse(Console.ReadLine(), out godineIskustvaBatlera) || godineIskustvaBatlera < 0)
                        {
                            Console.WriteLine("Unos nije ispravan. Molimo unesite cijeli broj veći od 0.");
                        }
                        novoOsoblje = new Batler(ime, prezime, DateTime.Now, plata, godineIskustvaBatlera);
                        break;

                    case "vrtlar":
                        Console.WriteLine("Unesite površinu nenamještenog stana za vrtlara:");
                        int povrsinaStana;
                        while (!int.TryParse(Console.ReadLine(), out povrsinaStana) || povrsinaStana < 0)
                        {
                            Console.WriteLine("Unos nije ispravan. Molimo unesite cijeli broj veći od 0.");
                        }

                        Console.WriteLine("Je li stan nenamješten? (da/ne):");
                        bool nenamjestenStan = Console.ReadLine().ToLower() == "da";

                        // Stvaranje instance Vrtlar klase s dodatnim podacima o stanu
                        novoOsoblje = new Vrtlar(ime, prezime, DateTime.Now, plata, povrsinaStana, nenamjestenStan);
                        break;

                    case "kuhar":
                        novoOsoblje = new Kuhar(ime, prezime, DateTime.Now, plata);
                        Console.WriteLine("Unesite jela koja kuhar može pripremiti (odvojena zarezima):");
                        string jela = Console.ReadLine();

                        // Razdvoji unesena jela zarezom
                        string[] listaJela = jela.Split(',');
                        break;
                }

                // Provjera je li novoOsoblje inicijalizirano prije nego što se koristi
                if (novoOsoblje != null)
                {
                    agencija.DodajOsoblje(novoOsoblje);
                    Console.WriteLine("Osoblje dodano!");
                }
                else
                {
                    Console.WriteLine("Greška pri dodavanju osoblja.");
                }
            }
        } while (dodajOsobljeOdgovor == "da");

        // Unos minimalne i maksimalne površine
        int minPovrsina = 0;
        int maxPovrsina = 0;

        Console.WriteLine("\nUnesite minimalnu željenu površinu");
        while (!Int32.TryParse(Console.ReadLine(), out minPovrsina) || minPovrsina < 0)
        {
            Console.WriteLine("Unos nije ispravan");
        }

        Console.WriteLine("\nUnesite maksimalnu željenu površinu");
        while (!Int32.TryParse(Console.ReadLine(), out maxPovrsina) || maxPovrsina < 0)
        {
            Console.WriteLine("Unos nije ispravan");
        }

        // Ispisivanje stanova koji zadovoljavaju kriterije površine
        Console.WriteLine($"\nStanovi između {minPovrsina} i {maxPovrsina} kvadratnih metara:");
        agencija.IspisiStanoveSortiranePoPovrsini(minPovrsina, maxPovrsina);

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

class Agencija
{
    private List<Stan> stanovi = new List<Stan>();
    private List<Osoba> osoblje = new List<Osoba>();

    public List<Osoba> DohvatiOsoblje()
    {
        return osoblje;
    }

    public void DodajStan(Stan stan, List<Osoba> osobljeNaImanju = null)
    {
        if (stan is LuksuzniApartman luksuzniApartman && osobljeNaImanju != null)
        {
            luksuzniApartman.PostaviOsobljeNaImanju(osobljeNaImanju);
        }

        stanovi.Add(stan);
    }

    public void IspisiSveStanoveSortiranePoCijeni()
    {
        var sortiraniStanovi = stanovi.OrderBy(s => s.ObracunajCijenuNajma()).ToList();

        foreach (Stan stan in sortiraniStanovi)
        {
            stan.Ispisi();
            Console.WriteLine($"Ukupna cijena najma stana je {stan.ObracunajCijenuNajma():F2}.\n");
        }
    }

    public void DodajOsoblje(Osoba osoba)
    {
        osoblje.Add(osoba);
    }

    public void IspisiSveOsobljeSortiranoPoPlati()
    {
        var sortiranoOsoblje = osoblje.OrderBy(o => o.Plata).ToList();

        foreach (Osoba osoba in sortiranoOsoblje)
        {
            osoba.Ispisi();
            Console.WriteLine();
        }
    }

    public void IspisiStanoveSortiranePoPovrsini(int minPovrsina, int maxPovrsina)
    {
        var filtriraniStanovi = stanovi
            .Where(s => s.BrojKvadrata >= minPovrsina && s.BrojKvadrata <= maxPovrsina)
            .OrderBy(s => s.BrojKvadrata)
            .ToList();

        foreach (Stan stan in filtriraniStanovi)
        {
            stan.Ispisi();
            Console.WriteLine($"Ukupna cijena najma stana je {stan.ObracunajCijenuNajma():F2}.\n");
        }
    }
}


abstract class Osoba
{
    public string Ime { get; }
    public string Prezime { get; }
    public DateTime DatumUposlenja { get; }
    public decimal Plata { get; }

    public Osoba(string ime, string prezime, DateTime datumUposlenja, decimal plata)
    {
        Ime = ime;
        Prezime = prezime;
        DatumUposlenja = datumUposlenja;
        Plata = plata;
    }

    public abstract void Ispisi();
}

class Batler : Osoba
{
    public int GodineIskustva { get; set; }

    public void PostaviGodineIskustva(int godineIskustva)
    {
        GodineIskustva = godineIskustva;
    }

    public Batler(string ime, string prezime, DateTime datumUposlenja, decimal plata, int godineIskustva)
        : base(ime, prezime, datumUposlenja, plata)
    {
        GodineIskustva = godineIskustva;
    }

    public override void Ispisi()
    {
        Console.WriteLine($"Batler: {Ime} {Prezime}, Datum uposlenja: {DatumUposlenja.ToShortDateString()}, Plata: {Plata:F2}, Godine iskustva: {GodineIskustva}");
    }
}

class Kuhar : Osoba
{
    public List<string> ListaJela { get; private set; }
    public void DodajJelo(string jelo)
    {
        ListaJela.Add(jelo);
    }
    public Kuhar(string ime, string prezime, DateTime datumUposlenja, decimal plata)
        : base(ime, prezime, datumUposlenja, plata)
    {
        ListaJela = new List<string>();
    }

    public override void Ispisi()
    {
        Console.WriteLine($"Kuhar: {Ime} {Prezime}, Datum uposlenja: {DatumUposlenja.ToShortDateString()}, Plata: {Plata:F2}");
        Console.WriteLine("\nJela koja mogu pripremiti:");
        foreach (var jelo in ListaJela)
        {
            Console.WriteLine(jelo);
        }
    }
}

class Vrtlar : Osoba
{
    public int PovrsinaStana { get; }
    public bool NenamjestenStan { get; }

    public Vrtlar(string ime, string prezime, DateTime datumUposlenja, decimal plata, int povrsinaStana, bool nenamjestenStan)
        : base(ime, prezime, datumUposlenja, plata)
    {
        PovrsinaStana = povrsinaStana;
        NenamjestenStan = nenamjestenStan;
    }

    public override void Ispisi()
    {
        Console.WriteLine($"Vrtlar: {Ime} {Prezime}, Datum uposlenja: {DatumUposlenja.ToShortDateString()}, Plata: {Plata:F2}");
        Console.WriteLine($"Površina stana: {PovrsinaStana} kvadrata, Stan nenamješten: {NenamjestenStan}");
    }
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

class LuksuzniApartman : Stan
{
    public decimal VrijednostNamjestaja { get; }
    public int BrojAparata { get; }
    private List<Osoba> osobljeNaImanju;

    public void PostaviOsobljeNaImanju(List<Osoba> osobljeNaImanju)
    {
        this.osobljeNaImanju = osobljeNaImanju;
    }

    public LuksuzniApartman(int brojKvadrata, Lokacija lokacija, bool internet, decimal vrijednostNamjestaja, int brojAparata, List<Osoba> osobljeNaImanju)
    : base(brojKvadrata, lokacija, internet)
    {
        VrijednostNamjestaja = vrijednostNamjestaja;
        BrojAparata = brojAparata;
        this.osobljeNaImanju = osobljeNaImanju;
    }

    public override void Ispisi()
    {
        Console.WriteLine($"{BrojKvadrata} {Lokacija} Luksuzni apartman {Internet} {VrijednostNamjestaja} {BrojAparata}");
    }

    public override decimal ObracunajCijenuNajma()
    {
        decimal osnovnaCijena = 1500M;
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

        // Dodajte plate osoblja na imanju
        foreach (Osoba osoba in osobljeNaImanju)
        {
            cijena += osoba.Plata;
        }

        return cijena + namjestajPovecanje;
    }
}