# Posouzení: přepsat UWP aplikaci na „klasickou“ Windows aplikaci

## Shrnutí

Krátká odpověď: **ano, je to možné**, ale je důležité upřesnit, co přesně znamená „klasická“ aplikace.

V tomto repozitáři už dnes běží hlavní aplikace Terminálu jako **Win32 proces** (`wWinMain`, `HWND`, vlastní message loop), který hostuje XAML UI (XAML Islands / WinUI 2).
To znamená, že nejde o „čistou UWP aplikaci“ v tradičním smyslu.

## Co je v kódu dnes

1. **Host je Win32 aplikace**
   - Projekt `WindowsTerminal` je `Win32Proj` a typ `Application`.
   - Kód používá `wWinMain` a pracuje přímo s `HWND`.

2. **UI vrstva je XAML (WinUI 2 / UWP-like)**
   - V procesu je explicitní práce s `DesktopWindowXamlSource` (XAML Islands).
   - Build kopíruje `Microsoft.UI.Xaml.dll` do unpackaged layoutu.

3. **Repo už řeší i unpackaged/portable scénáře**
   - Build obsahuje cíle pro „unpackaged run“.
   - Specifikace popisuje oficiální podporu unpackaged execution a portable mode.

## Co z toho plyne pro „přepsání na klasickou appku“

### Varianta A: „Klasická distribuce/spuštění“ (bez závislosti na MSIX/UWP identity)

- **Vysoká proveditelnost / nízké riziko**.
- Tato cesta je už prakticky rozpracovaná a částečně implementovaná.
- Nevyžaduje přepis UI stacku.

### Varianta B: „Klasická UI technologie“ (čisté Win32/WPF/WinUI 3 bez UWP XAML stacku)

- **Technicky proveditelné, ale drahé**.
- Znamená rozsáhlý zásah do `TerminalApp`/`TerminalControl` a hostingu UI.
- Přijdete o část existujících XAML komponent a bude potřeba náhrada:
  - ovládací prvky a šablony,
  - bindingy, styly, theme pipeline,
  - přístupnost a automatizační vrstvy,
  - testy UI a část build/deployment logiky.

## Odhad dopadu (hrubě)

- **A (unpackaged classic runtime):** týdny až nízké jednotky měsíců podle cíle distribuce.
- **B (kompletní přepis UI):** spíše **více měsíců až kvartály** a vyšší regresní riziko.

## Doporučení

1. Nejprve si potvrdit cíl:
   - pokud jde o instalaci/spouštění mimo Store/MSIX => pokračovat v unpackaged cestě,
   - pokud jde o opuštění XAML/WinUI => připravit samostatný migrační projekt s PoC.
2. Udělat PoC „kritické obrazovky“ (taby + pane + command palette) v cílové UI technologii.
3. Rozhodnout podle metrik: výkon startu, paměť, dostupnost funkcí, náklady na údržbu.

## Jednovětý závěr

**Smysluplné to je hlavně tehdy, pokud cílem je klasické (unpackaged) nasazení; kompletní přepis UI z UWP/XAML na jinou „klasickou“ technologii je možný, ale nákladný a rizikový.**
