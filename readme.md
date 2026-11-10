# MLBB Stream Overlay 🎮

Aplikasi *full-stack* ringan untuk menampilkan statistik Mobile Legends (Rank, Target Bintang, Winrate, dan Winstreak) secara *real-time* di OBS Studio. Didesain khusus untuk kebutuhan live streaming dengan admin panel yang praktis.

Proyek ini dirancang agar *frontend* React dapat diintegrasikan langsung ke dalam folder `wwwroot` milik *backend* .NET, sehingga hasil akhirnya bisa di-*build* menjadi **satu file executable (.exe)** yang sangat praktis dijalankan saat *streaming* tanpa perlu membuka kode editor.

## ✨ Fitur Utama
* **Real-time Updates:** Menggunakan koneksi WebSockets dari SignalR, setiap angka yang diubah di Admin Panel akan langsung ter-update di layar OBS tanpa perlu *refresh* browser.
* **Auto-Calculation:** Persentase *Winrate* (WR), sisa bintang menuju target, dan animasi progress bar dihitung secara otomatis.
* **Dynamic Rank Logo:** Logo rank pada *overlay* akan berubah secara dinamis menyesuaikan pilihan dari *dropdown*.
* **Data Persistence:** Status terakhir (jumlah bintang, rank, win streak) otomatis tersimpan di file lokal `overlay_data.json` agar data tidak hilang saat aplikasi ditutup.
* **Single Executable Ready:** UI dan API berjalan beriringan di satu port yang sama.

## 🛠️ Tech Stack
* **Backend:** C# .NET 8 Minimal API, SignalR
* **Frontend:** React JS (Vite), Tailwind CSS v4, Axios
* **Broadcasting:** OBS Studio (Browser Source)

## 🚀 Cara Build & Menjalankan dari Source Code

**1. Build Frontend (React)**
* Masuk ke folder *frontend* melalui terminal.
* Jalankan `npm install` untuk mengunduh *dependencies*.
* Jalankan `npm run build` untuk mengkompilasi kode React.
* Salin (*copy*) seluruh isi dari dalam folder `dist` yang baru saja terbentuk.

**2. Integrasi ke Backend (.NET)**
* Masuk ke folder *backend* (.NET).
* Buat folder baru bernama `wwwroot` (jika belum ada).
* Tempel (*paste*) semua isi folder `dist` tadi ke dalam folder `wwwroot`.

**3. Publish & Jalankan**
* Di terminal folder *backend*, jalankan perintah publish:
  ```bash
  dotnet publish -c Release -o ./publish
  ```
* Buka folder `publish` yang baru terbuat, cari file `.exe` aplikasi (misal: `MlbbOverlay.Backend.exe`), lalu jalankan.
* Server akan berjalan (default: `http://localhost:5000`) dan melayani UI sekaligus API secara bersamaan.

## 📺 Integrasi OBS & Cara Penggunaan Saat Stream

**Langkah 1: Memasang Overlay di OBS**
1. Buka OBS Studio, pada panel **Sources** (Sumber) klik ikon **+** lalu pilih **Browser** (Browser Source).
2. Beri nama source (misal: "MLBB Stats Overlay") dan klik OK.
3. Di jendela *Properties*, masukkan URL: `http://localhost:5000`
4. Atur **Width**: `800` dan **Height**: `250` (atau `300`).
5. Hapus semua teks yang ada di kolom **Custom CSS** (biarkan kosong agar *background* transparan bawaan kode bekerja maksimal).
6. Klik OK.

**Langkah 2: Menghilangkan Tombol Navigasi (Cropping)**
Karena aplikasi memuat tombol navigasi di bagian atas yang ikut ter-render, potong (*crop*) tombol tersebut agar tidak terlihat di *stream*:
* Klik *source* overlay di layar *preview* OBS sampai muncul garis merah di pinggirannya.
* Tahan tombol **Alt** pada keyboard, lalu klik dan geser (*drag*) garis merah bagian atas ke arah bawah sampai tombol menu tertutup. Garis akan berubah warna menjadi hijau sebagai tanda berhasil di-*crop*.

**Langkah 3: Mengontrol Data Saat Live Streaming**
* Buka *web browser* biasa (Chrome/Edge) di layar atau monitor kedua lu.
* Akses URL `http://localhost:5000`.
* Pilih menu **Admin Panel**.
* Lu dapat mengubah angka statistik (Bintang, Win Streak, dll) dan mengganti logo Rank dari form ini. 
* Klik **Push Update** dan tampilan *overlay* di OBS akan otomatis bergeser dan ter-update secara *real-time*!