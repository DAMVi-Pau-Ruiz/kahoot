using UnityEngine;

public static class KahootValidator
{
    public static bool TryValidate(string json, out string error)
    {
        error = "";

        KahootData kahoot;

        try
        {
            kahoot = JsonUtility.FromJson<KahootData>(json);
        }
        catch
        {
            error = "El JSON no tiene un formato válido";
            return false;
        }

        if (kahoot == null)
        {
            error = "No se ha podido leer el Kahoot";
            return false;
        }

        if (string.IsNullOrEmpty(kahoot.titulo) || string.IsNullOrEmpty(kahoot.descripcion))
        {
            error = "Faltan los campos de titulo o descripción.";
            return false;
        }

        if (kahoot.preguntas == null || kahoot.preguntas.Count < 1)
        {
            error = "El kahoot debe tener al menos una pregunta.";
            return false;
        }

        for (int i = 0; i < kahoot.preguntas.Count; i++)
        {
            PreguntaData pregunta = kahoot.preguntas[i];

            if (pregunta == null || string.IsNullOrEmpty(pregunta.pregunta))
            {
                error = "La pregunta " + (i + 1) + " no tiene un enunciado válido.";
                return false;
            }

            if (pregunta.respuestas == null || pregunta.respuestas.Count < 2 || pregunta.respuestas.Count > 4)
            {
                error = "La pregunta " + (i + 1) + " debe tener entre 2 y 4 respuestas.";
                return false;
            }

            int correctas = 0;
            int incorrectas = 0;

            foreach (RespuestaData respuesta in pregunta.respuestas)
            {
                if (respuesta == null || respuesta.texto == null)
                {
                    error = "Hay una respuesta sin texto en la pregunta " + (i + 1) + ".";
                    return false;
                }

                if (respuesta.correcta)
                {
                    correctas++;
                }
                else
                {
                    incorrectas++;
                }
            }

            if (correctas < 1)
            {
                error = "La pregunta " + (i + 1) + " necesita al menos una respuesta correcta.";
                return false;
            }

            if (incorrectas < 1)
            {
                error = "La pregunta " + (i + 1) + " necesita al menos una respuesta incorrecta.";
                return false;
            }

        }

        return true;
    }
}
