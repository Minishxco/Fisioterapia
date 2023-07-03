import json

# Crear una lista vacía para almacenar las preguntas
preguntas = []

# Inicializar las variables para almacenar el índice de cada pregunta y respuesta
lineapregunta = 2
respuestaA = 3
respuestaB = 4
respuestaC = 5
respuestaD = 6

with open("Preguntas.txt", "r", encoding="utf-8") as f:
    for i, line in enumerate(f, 1):
        if i == lineapregunta:
            pregunta = line
            lineapregunta += 7
        elif i == respuestaA:
            respuestas = []
            respuestas.append({"correcta": False, "respuesta": line})
            respuestaA += 7
        elif i == respuestaB:
            respuestas.append({"correcta": False, "respuesta": line})
            respuestaB += 7
        elif i == respuestaC:
            respuestas.append({"correcta": False, "respuesta": line})
            respuestaC += 7
        elif i == respuestaD:
            respuestas.append({"correcta": False, "respuesta": line})
            respuestaD += 7
            preguntas.append({"id": len(preguntas), "pregunta": pregunta, "Respuesta": respuestas, "DescRespuesta": ""})

# Leer archivo de descripciones
with open("Clasificacion.txt", "r") as f:
    descripciones = f.readlines()

# Asignar descripciones a preguntas
for i, pregunta in enumerate(preguntas):
    if i < len(descripciones):
        pregunta["DescRespuesta"] = descripciones[i].strip()
    else:
        break

# Leer archivo de respuestas correctas
with open("Respuestas.txt", "r") as f:
    respuestas_correctas = f.readlines()

# Asignar respuestas correctas a preguntas
for i, pregunta in enumerate(preguntas):
    if i < len(respuestas_correctas):
        for j, respuesta in enumerate(pregunta["Respuesta"]):
            if respuestas_correctas[i] == "A\n" and j == 0:
                respuesta["correcta"] = True
            elif respuestas_correctas[i] == "B\n" and j == 1:
                respuesta["correcta"] = True
            elif respuestas_correctas[i] == "C\n" and j == 2:
                respuesta["correcta"] = True
            elif respuestas_correctas[i] == "D\n" and j == 3:
                respuesta["correcta"] = True
    else:
        break

# Crear diccionario para envolver la lista de preguntas
data = {"Pregunta": preguntas}

# Guardar el JSON con el diccionario completo
with open("Fisioterapiadata.json", "w", encoding="utf-8") as f:
    json.dump(data, f, ensure_ascii=False, indent=4)
