
using System;
namespace BusinessEngine.Models
{
    public class InfosFacture : Entity
    {
        virtual public int Id { get; set; } 
        virtual public string Titre { get; set; } 
        virtual public string Adresse { get; set; } 
        virtual public string Ligne1 { get; set; } 
        virtual public string Ligne2 { get; set; } 
        virtual public string Ligne3 { get; set; } 
        virtual public string Message1 { get; set; } 
        virtual public string Message2 { get; set; } 
        virtual public string Message3 { get; set; } 
    }
}
