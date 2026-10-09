# 🚀 Galaxian Clone (`enes-sen-galaxianclone`)

![Unity](https://img.shields.io/badge/Unity-2021.3%2B-blue?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-10.0-green?style=for-the-badge&logo=c-sharp)
![License](https://img.shields.io/badge/License-MIT-orange?style=for-the-badge)

Klasik **Galaxian** arcade oyununun Unity oyun motoru ve C# dili kullanılarak modern, modüler ve yüksek performanslı yazılım mimarisi ilkeleriyle (SOLID) geliştirilmiş yeniden yapımıdır.

---

## 📌 Öne Çıkan Özellikler

* **⚡ Nesne Havuzu (Object Pooling):** Mermiler, düşmanlar ve patlama efektleri gibi sık oluşturulup yok edilen nesneler için bellek yönetimini (GC Spike) optimize eden özel nesne havuzu mimarisi (`PoolManagment.cs`).
* **📊 Veri Odaklı Tasarım (Data-Driven Design):** Düşman tipleri (`BaseEnemySO`) ve dalga düzenleri (`WaveLayoutSO`) ScriptableObject kullanılarak kod bağımsız şekilde yapılandırılmıştır.
* **🧩 Modüler ve Esnek Mimari:** `IEnemy`, `IDieable`, `IMove`, `IShoot` ve `IPool` gibi arayüzler (Interfaces) kullanılarak bileşenler arası bağımlılık (Loose Coupling) minimize edilmiştir.
* **🛸 Formasyon ve Dalga Sistemi:** Düşmanların ekran üzerindeki duruş dizilimlerini ve saldırı zamanlamalarını yöneten gelişmiş formasyon kontrolcüsü (`FormationManager`, `WaveManager`).
* **💥 Ses ve Görsel Efekt Yönetimi:** Efektlerin ve seslerin merkezi yönetimini sağlayan modüler sistemler (`VFXManager`, `SFXManager`, `ExplosionVFXManager`).

---

## 📁 Proje Dizin Yapısı

```text
Assets/
└── Projects/
    ├── Animations/            # Düşman ve oyuncu animasyonları
    ├── Materials/             # Materyal ve Kaplamalar
    ├── Prefabs/               # Prefabrik Nesneler (Player, Enemies, Bullets, VFX)
    ├── Scenes/
    │   ├── Game.unity         # Ana Oyun Sahnesi
    │   └── Menu.unity         # Giriş ve Menü Sahnesi
    ├── ScriptableObjects/     # Düşman ve Dalga Ayar Dosyaları (.asset)
    ├── Scripts/
    │   ├── Combat/            # Savaş, Mermi ve Hasar Mekanizmaları
    │   ├── Interfaces/        # Arayüz Tanımlamaları (IPool, IEnemy, IMove, vb.)
    │   ├── Managers/          # Oyun, Skor, Girdi ve Formasyon Yöneticileri
    │   ├── RuntimeSupport/    # Object Pooling, VFX ve SFX Sistemleri
    │   └── ScriptableObject/  # SO Sınıf Tanımlamaları
    ├── Sound/                 # Ses Efektleri ve Arka Plan Müzikleri
    └── Sprites/               # 2D Görsel Varlıklar
```

---

## 🛠️ Sistem Mimarisi

```text
                         +-------------------+
                         |   GameController  |
                         +---------+---------+
                                   |
         +-------------------------+-------------------------+
         |                         |                         |
+--------v-------+        +--------v-------+        +--------v-------+
|  InputManager  |        |  WaveManager   |        | ScoreManager   |
+----------------+        +--------+-------+        +----------------+
                                   |
                          +--------v-------+
                          | FormationManager|
                          +--------+-------+
                                   |
                          +--------v-------+
                          | PoolManagement |
                          +----------------+
```

---

## 🎮 Kontroller

| Eylem | Klavye / Girdi |
| :--- | :--- |
| **Sola / Sağa Hareket** | `A / D` veya `Sol / Sağ Yön Tuşları` |
| **Ateş Etme** | `Space (Boşluk Tuşu)` |

---

## 🚀 Kurulum ve Çalıştırma

1. **Projeyi Kllonlayın:**
   ```bash
   git clone https://github.com/enes-sen/enes-sen-galaxianclone.git
   ```
2. **Unity Hub ile Açın:**
   * Unity Hub'ı açın ve **Add project from disk** seçeneği ile indirilen klasörü seçin.
   * Önerilen Unity Sürümü: **2021.3 LTS** veya üstü.
3. **Sahneyi Başlatın:**
   * `Assets/Projects/Scenes/Menu.unity` sahnesini açın ve **Play** butonuna basın.

---

## 📄 Lisans

Bu proje kişisel gelişim ve eğitim amacıyla geliştirilmiştir. MIT Lisansı kapsamında özgürce kullanılabilir ve geliştirilebilir.
