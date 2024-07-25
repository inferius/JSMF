namespace JSMF.Parser.Tokenizer
{
    public enum TokenType
    {
        Separator, // oddělovače zavorka, carka, strednik, ...
        Numeric, // číslo
        String, // řetezec
        Keyword, // klicove slovo
        Identifier, // identifikatory (nazev promenne, nazev funkce)
        Operator, // operatory (!=,++)
        EndToken // ukončuje čtení tokenu
    }
}
