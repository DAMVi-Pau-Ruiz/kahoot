using System;
using System.Collections.Generic;

[Serializable]
public class KahootData
{
    public string titulo;
    public string descripcion;
    public string imagen;
    public List<PreguntaData> preguntas;
}