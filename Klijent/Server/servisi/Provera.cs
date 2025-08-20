using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Server.servisi
{
    public class Provera
    {

        public (bool i, string j) ProveriPrijavu(string poruka, IPEndPoint serverTCPEP, ref Igrac igrac, ref string[] igre)
        {
           
            string[] dozvoljeno = { "sl", "sk", "kzz" };

            if (string.IsNullOrWhiteSpace(poruka) || !poruka.StartsWith("PRIJAVA:"))
            {
                return (false, "GRESKA: Pogresan format poruke. Ocekivano: PRIJAVA:ime, niz igara koje zelite da igrate");
            }

            string ostatak = poruka.Substring("PRIJAVA:".Length).Trim();
            string[] delovi = ostatak.Split(',');

            if (delovi.Length < 2)
            {
                return (false, "GRESKA: Pogresan format poruke. Ocekivano: PRIJAVA:ime, niz igara koje zelite da igrate");
            }
            if (delovi.Length > 4)
            {
                return (false, "GRESKA: Previse igara. Maksimalno 3 igre.");
            }

            string ime = delovi[0].Trim();
            igre = delovi.Skip(1).Select(i => i.ToLower().Trim()).ToArray();

            if (!igre.All(i => dozvoljeno.Contains(i)))
            {
                return (false, "GRESKA: Dozvoljene igre su samo sl, sk, kzz");
            }
            if (igre.Count(i => i == "sk") > 2)
            {
                return (false, "GRESKA: 'sk' se može navesti najviše 2 puta");
            }
            if (igre.Count(i => i == "sl") > 1)
            {
                return (false, "GRESKA: 'sl' se može navesti najviše jednom");
            }
            if (igre.Count(i => i == "kzz") > 1)
            {
                return (false, "GRESKA: 'kzz' se može navesti najviše jednom");
            }

            // ako je sve validno
            string endpoint = serverTCPEP.ToString();
            var random = new Random();
            int id = random.Next(1000, 9999);
            int[] nizPoena = new int[igre.Length];
            igrac = new Igrac(id, ime, nizPoena);

            Console.WriteLine($"Igrač {ime}, igre: {string.Join(",", igre)}");

            return (true, $"TCP: {endpoint}");
        }


        public Provera()
        {
        }
       


        // Vrati: true ako je validno; out guess je normalizovan (UPPERCASE, bez razmaka),
        // out greska je poruka za klijenta ako NIJE validno.
        public  (bool, string) ProveraSkocko(string input)
        {
            
            if (string.IsNullOrWhiteSpace(input))
            {
               
                return (false, "GRESKA:Prazan pokušaj.");
            }

            string g = input.Replace(" ", "").ToUpperInvariant();
            const string dozvoljeno = "HTPKSZ";
            for (int i = 0; i < g.Length; i++)
            {
                char c = g[i];
                if (dozvoljeno.IndexOf(c) < 0)
                {

                    return (false, "GRESKA:Dozvoljeni znakovi su: H, T, P, K, S, Z.");
                }
            }
            // Ukloni razmake i normalizuj u velika slova
            

            if (g.Length != 4)
            {
                
                return (false, "GRESKA:Pokušaj mora imati tačno 4 znaka.");
            }

            

            
            return (true, "ok");
        }
    }


















}

