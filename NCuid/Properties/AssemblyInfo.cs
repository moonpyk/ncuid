using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Les informations générales relatives à un assembly dépendent de 
// l'ensemble d'attributs suivant. Changez les valeurs de ces attributs pour modifier les informations
// associées à un assembly.
[assembly: AssemblyTitle("Moonpyk.NCuid")]
// [assembly: AssemblyDescription("")]
// [assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Moonpyk")]
[assembly: AssemblyProduct("Moonpyk.NCuid")]
[assembly: AssemblyCopyright("Copyright 2014 © Clément Bourgeois")]
// [assembly: AssemblyTrademark("")]
// [assembly: AssemblyCulture("")]

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly 
// aux composants COM.  Si vous devez accéder à un type dans cet assembly à partir de 
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

// Le GUID suivant est pour l'ID de la typelib si ce projet est exposé à COM
[assembly: Guid("ca82e0f7-976a-4f3c-bb29-1640af785b97")]

#if DEBUG
  [assembly: InternalsVisibleTo("NCuid.Tests")]
#endif
