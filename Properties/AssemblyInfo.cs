using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using YouTubeTVMod;

// Informations générales sur l'assembly contrôlées via
// l'ensemble d'attributs suivant. Changez ces valeurs d'attribut pour modifier
// les informations associées à un assembly.
[assembly: AssemblyTitle("BoomboxMod")]
[assembly: AssemblyDescription("Play YouTube videos on in-game TVs")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("VotreNomOuPseudo")] // Mettez votre nom ici
[assembly: AssemblyProduct("BoomboxMod")]
[assembly: AssemblyCopyright("Copyright © VotreNomOuPseudo 2024")] // Mettez votre nom ici
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly
// aux composants COM. Si vous devez accéder à un type dans cet assembly à partir de
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
// !!! IMPORTANT: Generate a new GUID and replace the placeholder below !!!
// You can use Visual Studio Tools -> Create GUID, or an online generator.
[assembly: Guid("70d8e3c5-8248-4946-a9d1-c78d170c90c1")]

// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire
//      Numéro de build
//      Révision
//
// Vous pouvez spécifier toutes les valeurs ou indiquer les numéros de build et de révision par défaut
// en utilisant '*', comme indiqué ci-dessous :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Attributs MelonLoader
[assembly: MelonInfo(typeof(YouTubeTVMod.BoomboxMod), "Boombox Mod", "1.0.0", "VotreNomOuPseudo")]
[assembly: MelonGame("TVGS", "Schedule I")] // Remplacez par les infos exactes du jeu si connues 