using UnityEngine;

public static class KahootValidator
{
    public static bool TryValidate(string json, string kahootFileName, out string error)
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

            ErrorManager.Instance.RegisterError(kahootFileName, "JSON mal formado", error);
            
            return false;
        }

        if (kahoot == null)
        {
            error = "No se ha podido leer el Kahoot";

            ErrorManager.Instance.RegisterError(kahootFileName, "JSON No encontrado", error);

            return false;
        }

        if (string.IsNullOrEmpty(kahoot.titulo) || string.IsNullOrEmpty(kahoot.descripcion))
        {
            error = "Faltan los campos de titulo o descripción.";

            ErrorManager.Instance.RegisterError(kahootFileName, "Kahoot mal formado", error);

            return false;
        }

        if (kahoot.preguntas == null || kahoot.preguntas.Count < 1)
        {
            error = "El kahoot debe tener al menos una pregunta.";

            ErrorManager.Instance.RegisterError(kahootFileName, "Kahoot sin preguntas", error);

            return false;
        }

        for (int i = 0; i < kahoot.preguntas.Count; i++)
        {
            PreguntaData pregunta = kahoot.preguntas[i];

            if (pregunta == null || string.IsNullOrEmpty(pregunta.pregunta))
            {
                error = "La pregunta " + (i + 1) + " no tiene un enunciado válido.";

                ErrorManager.Instance.RegisterError(kahootFileName, "Pregunta mal formada", error);

                return false;
            }

            if (pregunta.respuestas == null || pregunta.respuestas.Count < 2 || pregunta.respuestas.Count > 4)
            {
                error = "La pregunta " + (i + 1) + " debe tener entre 2 y 4 respuestas.";

                ErrorManager.Instance.RegisterError(kahootFileName, "Respuestas incorrectas", error);

                return false;
            }

            int correctas = 0;
            int incorrectas = 0;

            foreach (RespuestaData respuesta in pregunta.respuestas)
            {
                if (respuesta == null || respuesta.texto == null)
                {
                    error = "Hay una respuesta sin texto en la pregunta " + (i + 1) + ".";

                    ErrorManager.Instance.RegisterError(kahootFileName, "Respuesta mal formada", error);

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

                ErrorManager.Instance.RegisterError(kahootFileName, "Sin respuestas correctas", error);

                return false;
            }

            if (incorrectas < 1)
            {
                error = "La pregunta " + (i + 1) + " necesita al menos una respuesta incorrecta.";

                ErrorManager.Instance.RegisterError(kahootFileName, "Sin respuestas incorrectas", error);

                return false;
            }

        }

        return true;
    }
}
