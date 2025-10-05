🌳 Desafío de Programación AVL

👨‍🎓 Información del Estudiante
Campo	Información
Alumno	Fernando Wilfredo Orellana Rodriguez
Universidad	Universidad Don Bosco
Materia	Programación con Estructura de Datos
Desafío	Desafío #1 - Árboles AVL
🎯 Descripción del Desafío

Este proyecto implementa una estructura de datos Árbol AVL (Adelson-Velsky y Landis), un tipo de árbol binario de búsqueda auto-balanceado que mantiene su altura balanceada automáticamente para garantizar operaciones eficientes.

¿Qué es un Árbol AVL?

Un árbol AVL es un árbol binario de búsqueda donde las alturas de los dos subárboles de cualquier nodo difieren en máximo una unidad. Esta propiedad garantiza que el árbol permanezca balanceado, proporcionando un rendimiento óptimo en las operaciones de búsqueda, inserción y eliminación.

⚡ Características Principales
✅ Auto-balanceado: Mantiene automáticamente el balance del árbol
✅ Operaciones eficientes: O(log n) para inserción, eliminación y búsqueda
✅ Rotaciones: Implementa rotaciones simples y dobles para mantener el balance
✅ Factor de balance: Calcula y mantiene el factor de balance de cada nodo
✅ Recorridos: Implementa recorridos in-order, pre-order y post-order
🔧 Operaciones Implementadas
Operaciones Básicas
Inserción: Añade un nuevo nodo manteniendo el balance
Eliminación: Remueve un nodo y rebalancea el árbol
Búsqueda: Encuentra un elemento específico en el árbol
Recorridos: Visita todos los nodos en diferentes órdenes
Operaciones de Balanceado
Rotación Simple Derecha (RR)
Rotación Simple Izquierda (LL)
Rotación Doble Izquierda-Derecha (LR)
Rotación Doble Derecha-Izquierda (RL)
📊 Complejidad Temporal
Operación	Complejidad
Búsqueda	O(log n)
Inserción	O(log n)
Eliminación	O(log n)
Recorrido	O(n)
🚀 Cómo Usar
# Compilar el programa
gcc -o avl_tree main.c avl.c

# Ejecutar
./avl_tree

📋 Consideraciones Importantes
⚠️ Puntos Clave a Tener en Cuenta:
Factor de Balance: Siempre debe estar entre -1, 0, y 1
Rotaciones: Son fundamentales para mantener el balance del árbol
Altura del Árbol: Se calcula recursivamente y se actualiza después de cada operación
Casos de Rotación: Identificar correctamente cuándo aplicar cada tipo de rotación
Memoria: Gestionar correctamente la asignación y liberación de memoria

🔍 Casos Especiales:
Inserción en árbol vacío
Eliminación del nodo raíz
Eliminación de nodos con uno o dos hijos
Manejo de duplicados (según implementación)

🧪 Testing:
- Probar con diferentes secuencias de inserción
- Verificar el balance después de cada operación
- Validar los recorridos del árbol
- Comprobar la correcta eliminación de nodos

📚 Recursos Adicionales
Documentación sobre Árboles AVL
Visualización de Árboles AVL
Algoritmos de Rotación
📝 Notas del Desarrollo

Este proyecto forma parte del aprendizaje de estructuras de datos avanzadas, enfocándose en la comprensión de algoritmos de auto-balanceado y su implementación práctica.

Universidad Don Bosco | Programación con Estructura de Datos | 2024

Desarrollado con 💻 y ☕ por Fernando Wilfredo Orellana Rodriguez
