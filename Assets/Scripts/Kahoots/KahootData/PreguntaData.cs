using System;
using System.Collections.Generic;

[Serializable]
public class PreguntaData
{
    public string pregunta;
    public string imagen;
    public int duracion;
    public List<RespuestaData> respuestas;
}