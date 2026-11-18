using System;
using System.Collections.Generic;

namespace MafiaGalonSystem
{

    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    // =========================================================================
    // 1. CLASS, OBJECT, ATTRIBUTE, DAN METHOD: Agen
    // =========================================================================
    public class Agen
    {
        public string idAgen { get; set; }
        public string namaAgen { get; set; }
        public int stokAgen { get; set; }

        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            this.idAgen = idAgen;
            this.namaAgen = namaAgen;
            this.stokAgen = stokAgen;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"[AGEN] ID: {idAgen} | Markas/Nama: {namaAgen} | Stok Saat Ini: {stokAgen} galon");
        }
    }

    // =========================================================================
    // 2. ENCAPSULATION & 5. ABSTRACT CLASS: ProdukGalon
    // =========================================================================
    public abstract class ProdukGalon
    {
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
            private set
            {
                if (value < 0)
                {
                    stokGudang = 0;
                }
                else
                {
                    stokGudang = value;
                }
            }
        }
        public ProdukGalon(string idProduk, string namaProduk, int stokAwal)
        {
            this.idProduk = idProduk;
            this.namaProduk = namaProduk;
            this.StokGudang = stokAwal < 0 ? 0 : stokAwal;
        }
        public bool KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stokGudang)
            {
                stokGudang -= jumlah;
                return true;
            }
            return false;
        }

        public abstract double HitungBiaya(int jumlah);

        public virtual void TampilProduk()
        {
            Console.WriteLine($"[PRODUK] ID: {IdProduk} | Nama: {NamaProduk} | Stok Gudang Pusat: {StokGudang} galon");
        }
    }

    // =========================================================================
    // 3. INHERITANCE & 4. POLYMORPHISM (OVERRIDING): GalonAkua
    // =========================================================================
    public class GalonAkua : ProdukGalon
    {
        public GalonAkua(string idProduk, string namaProduk, int stokAwal)
            : base(idProduk, namaProduk, stokAwal)
        {
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    // =========================================================================
    // 3. INHERITANCE & 4. POLYMORPHISM (OVERRIDING): GalonBerkeran
    // =========================================================================
    public class GalonBerkeran : ProdukGalon
    {
        public string JenisKeran { get; set; }

        public GalonBerkeran(string idProduk, string namaProduk, int stokAwal, string jenisKeran)
            : base(idProduk, namaProduk, stokAwal)
        {
            this.JenisKeran = jenisKeran;
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 12000;
        }

        public override void TampilProduk()
        {
            Console.WriteLine($"[PRODUK] ID: {IdProduk} | Nama: {NamaProduk} (Keran: {JenisKeran}) | Stok Gudang Pusat: {StokGudang} galon");
        }
    }

    // =========================================================================
    // 6. IMPLEMENTASI INTERFACE: DistribusiGalon
    // =========================================================================
    public class DistribusiGalon : DapatDidistribusikan
    {
        public Agen AgenTujuan { get; set; }
        public ProdukGalon ProdukDikirim { get; set; }
        public int Jumlah { get; set; }

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produkDikirim, int jumlah)
        {
            this.AgenTujuan = agenTujuan;
            this.ProdukDikirim = produkDikirim;
            this.Jumlah = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"\n--- Memproses Pengiriman {Jumlah}x {ProdukDikirim.NamaProduk} ke {AgenTujuan.namaAgen} ---");

            if (Jumlah <= 0)
            {
                Console.WriteLine("[GAGAL] Operasi dibatalkan! Jumlah galon yang dikirim harus lebih dari 0.");
                return;
            }

            if (Jumlah > ProdukDikirim.StokGudang)
            {
                Console.WriteLine($"[GAGAL] Pasokan tidak mencukupi! Permintaan ({Jumlah}) melebihi stok gudang ({ProdukDikirim.StokGudang}). Stok tetap aman.");
                return;
            }

            bool berhasilKurang = ProdukDikirim.KurangiStok(Jumlah);
            if (berhasilKurang)
            {
                AgenTujuan.stokAgen += Jumlah;

                double totalBiaya = ProdukDikirim.HitungBiaya(Jumlah);

                Console.WriteLine($"[SUKSES] Paket berhasil dikirimkan ke agen {AgenTujuan.namaAgen} ({AgenTujuan.idAgen})!");
                Console.WriteLine($"         Rincian: {Jumlah} unit {ProdukDikirim.NamaProduk} | Total Setoran Kartel: Rp{totalBiaya:N0}");
            }
        }
    }

    // =========================================================================
    // 7. OPSIONAL: SindikatGalon
    // =========================================================================
   
    public class SindikatGalon
    {
        private List<Agen> daftarAgen = new List<Agen>();
        private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            daftarAgen.Add(agen);
            Console.WriteLine($"[+] Agen baru '{agen.namaAgen}' resmi direkrut ke dalam sindikat.");
        }

        public void TambahProduk(ProdukGalon produk)
        {
            daftarProduk.Add(produk);
            Console.WriteLine($"[+] Pasokan '{produk.NamaProduk}' ditambahkan ke gudang pusat.");
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("         DATA ASET & JARINGAN SINDIKAT MAFIA GALON     ");
            Console.WriteLine("=======================================================");
            Console.WriteLine(">> DAFTAR AGEN AKTIF:");
            foreach (var agen in daftarAgen)
            {
                agen.TampilInfo();
            }

            Console.WriteLine("\n>> INVENTARIS GUDANG PUSAT:");
            foreach (var produk in daftarProduk)
            {
                produk.TampilProduk();
            }
            Console.WriteLine("=======================================================\n");
        }
    }

    // =========================================================================
    // MAIN PROGRAM
    // =========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEM MANAJEMEN DISTRIBUSI MAFIA GALON ===\n");

            Agen agen1 = new Agen("AG-001", "Markas Sektor Utara (Don Vito)", 10);
            Agen agen2 = new Agen("AG-002", "Bunker Pelabuhan Timur (Scarface)", 5);


            ProdukGalon galonAkua = new GalonAkua("PRD-01", "Air Galon Akua Murni", 50);
            ProdukGalon galonKeran = new GalonBerkeran("PRD-02", "Air Galon Berkeran Impor", 30, "Keran Pencet Stainless");

            SindikatGalon sindikat = new SindikatGalon();
            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);
            sindikat.TambahProduk(galonAkua);
            sindikat.TambahProduk(galonKeran);

            sindikat.TampilkanSindikat();

            Console.WriteLine("--- SIMULASI PERHITUNGAN BIAYA (POLYMORPHISM) ---");
            Console.WriteLine($"Estimasi 5 {galonAkua.NamaProduk}: Rp{galonAkua.HitungBiaya(5):N0}");
            Console.WriteLine($"Estimasi 5 {galonKeran.NamaProduk}: Rp{galonKeran.HitungBiaya(5):N0}");

            DapatDidistribusikan kirim1 = new DistribusiGalon(agen1, galonAkua, 15);
            kirim1.KirimPaket();

            DapatDidistribusikan kirim2 = new DistribusiGalon(agen2, galonKeran, 10);
            kirim2.KirimPaket();

            DapatDidistribusikan kirimGagal1 = new DistribusiGalon(agen1, galonAkua, 0);
            kirimGagal1.KirimPaket();

            DapatDidistribusikan kirimGagal2 = new DistribusiGalon(agen2, galonKeran, 100);
            kirimGagal2.KirimPaket();

            Console.WriteLine("\n--- KONDISI ASET SETELAH OPERASI DISTRIBUSI ---");
            sindikat.TampilkanSindikat();

            Console.ReadLine();
        }
    }
}