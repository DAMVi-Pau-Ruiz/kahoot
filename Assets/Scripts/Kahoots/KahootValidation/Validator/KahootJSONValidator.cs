
using System.Collections.Generic;
using UnityEngine;

public static class KahootJSONValidator
{
    public static List<string> Validate(string json)
    {
        List<string> errors = new List<string>();

        KahootData data = null;

        // 1. Comprobar que el JSON se puede leer
        try
        {
            data = JsonUtility.FromJson<KahootData>(json);
        }
        catch (System.Exception)
        {
            errors.Add("El archivo JSON no tiene un formato válido.");
            return errors;
        }

        if (data == null)
        {
            errors.Add("No se han podido obtener los datos del Kahoot.");
            return errors;
        }

        // 2. Comprobar los datos generales
        if (string.IsNullOrWhiteSpace(data.titulo))
            errors.Add("El Kahoot no tiene título.");

        if (string.IsNullOrWhiteSpace(data.descripcion))
            errors.Add("El Kahoot no tiene descripción.");

        if (string.IsNullOrWhiteSpace(data.imagen))
            errors.Add("El Kahoot no tiene imagen.");

        // 3. Comprobar las preguntas
        if (data.preguntas == null || data.preguntas.Count == 0)
        {
            errors.Add("El Kahoot debe tener al menos una pregunta.");
            return errors;
        }

        for (int i = 0; i < data.preguntas.Count; i++)
        {
            PreguntaData pregunta = data.preguntas[i];

            if (pregunta == null)
            {
                errors.Add("La pregunta " + (i + 1) + " no es válida.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(pregunta.pregunta))
                errors.Add("La pregunta " + (i + 1) + " no tiene texto.");

            if (pregunta.duracion <= 0)
                errors.Add("La pregunta " + (i + 1) + " tiene una duración inválida.");

            // 4. Comprobar las respuestas
            if (pregunta.respuestas == null ||
                pregunta.respuestas.Count < 2 ||
                pregunta.respuestas.Count > 4)
            {
                errors.Add("La pregunta " + (i + 1) +
                    " debe tener entre 2 y 4 respuestas.");

                continue;
            }

            bool hasCorrectAnswer = false;
            bool hasIncorrectAnswer = false;

            for (int j = 0; j < pregunta.respuestas.Count; j++)
            {
                RespuestaData respuesta = pregunta.respuestas[j];

                if (respuesta == null)
                {
                    errors.Add("La respuesta " + (j + 1) +
                        " de la pregunta " + (i + 1) + " no es válida.");

                    continue;
                }

                if (string.IsNullOrWhiteSpace(respuesta.texto))
                    errors.Add("La respuesta " + (j + 1) +
                        " de la pregunta " + (i + 1) + " no tiene texto.");

                if (respuesta.correcta)
                    hasCorrectAnswer = true;
                else
                    hasIncorrectAnswer = true;
            }

            if (!hasCorrectAnswer)
                errors.Add("La pregunta " + (i + 1) +
                    " debe tener al menos una respuesta correcta.");

            if (!hasIncorrectAnswer)
                errors.Add("La pregunta " + (i + 1) +
                    " debe tener al menos una respuesta incorrecta.");
        }

        return errors;
    }
}
