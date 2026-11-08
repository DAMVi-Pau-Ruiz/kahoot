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

[Serializable]
public class PreguntaData
{
    public string pregunta;
    public string duracion;

    public List<RespuestaData> respuestas;
}

[Serializable]
public class RespuestaData
{
    public string texto;
    public bool correcta;
}