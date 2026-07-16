# Cuestionario

## 1. ¿Qué significa "void" cuando se encuentra en una clase?

Que el método no devuelve nada. Solo ejecuta código pero no hay un `return valor`.

## 2. Corrige si existe algo mal en el código:

```csharp
switch (variable)
{
    case val1:
    case val2:
    default:
}
```

Le faltan los `break` y el contenido de cada case. Así:

```csharp
switch (variable)
{
    case val1:
        // lógica val1
        break;
    case val2:
        // lógica val2
        break;
    default:
        // lógica por defecto
        break;
}
```

## 3. ¿Qué es un namespace y da 3 ejemplos de namespaces en .NET?

Es una forma de agrupar clases relacionadas y evitar que se choquen nombres repetidos. Ejemplos:
- `System.Collections.Generic`
- `Microsoft.EntityFrameworkCore`
- `POS.Business.Rules`

## 4. ¿Cuál es la diferencia esencial entre la programación estructurada y la orientada a objetos?

Estructurada: funciones separadas de los datos, todo en secuencia de pasos. POO: los datos y el comportamiento van juntos dentro de objetos que interactúan entre sí.

## 5. Diferencia entre campos private, public e internal

- **private**: solo se ve dentro de la misma clase.
- **public**: se ve desde cualquier proyecto/ensamblado.
- **internal**: se ve solo dentro del mismo proyecto (ensamblado), no desde otros que lo referencien.

## 6. Definir los campos de las clases para grupos de empleados

Igual que en el proyecto tenemos `Product` y `ProductType` relacionados por FK, acá sería `Employee` y `Group`:

```csharp
public class Employee
{
    public string Name { get; set; }
    public int Code { get; set; } // rango 1-10000, se valida al asignar
    public string JobTitle { get; set; }
    public decimal BaseSalary { get; set; }
    public Employee? Supervisor { get; set; }   // referencia a su superior directo
    public Group Group { get; set; }            // grupo al que pertenece
}

public class Group
{
    public string OfficeName { get; set; }
    public int DepartmentCode { get; set; } // rango 1-23
    public List<Employee> Employees { get; set; } = new();
}
```

## 7. Defina con sus propias palabras que es un Framework o marco de trabajo. Mencione algunos ejemplos.

Un conjunto de librerías, herramientas y reglas ya armadas que te dan una base para no reinventar todo desde cero (manejo de conexiones, DI, ruteo, etc.). Se programás adentro de esas reglas en vez de armar la infraestructura. Ejemplos: .NET, Spring Boot (Java), Angular, Django.

## Mencione al menos 3 características más relevantes de las últimas versiones del .NET Framework >= 4.7

- **Nullable reference types**: el compilador te avisa si podés estar usando un objeto nulo sin chequear (`<Nullable>enable</Nullable>`, lo usamos en todos los `.csproj` del proyecto).
- **Top-level statements**: ya no hace falta la clase `Program` con `Main` explícito, se escribe directo (así está el `Program.cs` de `POS.Presentation`).
- **Generic Host / Minimal Hosting APIs**: `Host.CreateApplicationBuilder` para armar DI, configuración y logging con pocas líneas (lo usamos para registrar `AddPosExpress`).

## 9. Describe qué hace el método y corrija los errores

```csharp
public void metodo(int[] lista, int pos)
{
    int result = 0;
    for (i = pos; i < length; i++)
    {
        result += lista[i];
    }
    return result;
}
```

Suma los elementos del array `lista` desde el índice `pos` hasta el final. Errores:
- `i` no está declarado.
- `length` no existe, es `lista.Length`.
- El método es `void` pero hace `return result;` (no puede devolver nada).

Corregido:

```csharp
public int Metodo(int[] lista, int pos)
{
    int result = 0;
    for (int i = pos; i < lista.Length; i++)
    {
        result += lista[i];
    }
    return result;
}
```

## 10. Mostrar "farmacorp" al revés

```csharp
string palabra = "farmacorp";
char[] letras = palabra.ToCharArray();
Array.Reverse(letras);
Console.WriteLine(new string(letras));
// prcamraf
```

## 11. ¿Qué es el Common Language Runtime y para qué sirve?

Básicamente es lo que hace que el mismo código compilado corra sin que vos te preocupes por la memoria o el hardware.

## 12. Definir una clase de nombre Persona con las siguientes consideraciones:
a.
Atributos: nombre, edad, peso.
b.
Un método llamado Saludo que no reciba parámetros y que a su nombre le concatene al inicio la palabra "Hola_".
c.
Un método llamado Calcula que no reciba parámetros y devuelva el producto de su edad por su peso.

```csharp
public class Persona
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public double Peso { get; set; }

    public string Saludo()
    {
        return "Hola_" + Nombre;
    }

    public double Calcula()
    {
        return Edad * Peso;
    }
}
```

## 13. Escribe el código para declarar una instancia de la clase anterior, asignar valores a todos sus atributos, mostrar en pantalla el resultado de invocar a los métodos "Saludo" y "Calcula"

```csharp
Persona persona = new Persona();
persona.Nombre = "Edson";
persona.Edad = 32;
persona.Peso = 70;

Console.WriteLine(persona.Saludo());
Console.WriteLine(persona.Calcula());
```

## 14. Explique las 3 primeras formas normales de base de datos.

- **1FN**: cada campo tiene un solo valor y hay una clave que identifica cada fila.
- **2FN**: cumple 1FN y además todos los campos que no son clave dependen de la clave completa, no de una parte de ella (aplica cuando la clave es compuesta).
- **3FN**: cumple 2FN y además ningún campo que no es clave depende de otro campo que tampoco es clave (sin dependencias transitivas).

## 15. ¿Qué significa JOIN en una consulta de base de datos?

Es combinar filas de dos o más tablas relacionadas en una sola consulta, usando una columna en común (generalmente una FK). Por ejemplo, en el proyecto, para traer una venta junto con el nombre del producto se necesitaría un JOIN entre `ExpressSales` y `Products` por `ProductId`.

## 16. Escribe el código Sql para realizar las siguientes acciones:
a.
Crear una tabla de nombre "Productos" que tenga un campo: nombre, precio y fecha.
b.
Insertar el producto "Leche Pil" con fecha del 17 de noviembre de 2017 y precio de Bs 5.50.
c.
Cambiar el nombre del producto "Leche Pil" a "Leche Pil Deslactosada"
d.
Mostrar el nombre de todos los productos que cuesten más de Bs 6
e.
Borrar todos los productos donde el precio este comprendido entre Bs. 5 y 6

```sql
-- a
CREATE TABLE Productos (
    nombre VARCHAR(100),
    precio DECIMAL(10,2),
    fecha DATE
);

-- b
INSERT INTO Productos (nombre, precio, fecha)
VALUES ('Leche Pil', 5.50, '2017-11-17');

-- c
UPDATE Productos
SET nombre = 'Leche Pil Deslactosada'
WHERE nombre = 'Leche Pil';

-- d
SELECT nombre FROM Productos WHERE precio > 6;

-- e
DELETE FROM Productos WHERE precio BETWEEN 5 AND 6;
```

## 17. Actualización de versión de librería

Actualizaría a **v.1.1.4**. Es la siguiente versión patch, que ya trae el fix y es compatible hacia atrás, entonces resuelvo el bug sin arriesgar nada más. No iría directo a  v.2.1.3 porque  pueden traer cambios de comportamiento o breaking changes que no necesito.

## 18. Algoritmo para almacenar contraseñas

Nunca guardar la contraseña en texto plano ni con un hash simple (MD5/SHA1 solos). Lo correcto es un algoritmo de hashing pensado para contraseñas, con salt y costo configurable: **bcrypt**.

## 19. Métodos de autenticación web usados

- **JWT (Bearer token)**: el más común en APIs, el cliente manda el token en el header `Authorization` y el server lo valida sin guardar sesión.
- **Cookies/Session**: típico en apps web tradicionales, el server guarda la sesión y la cookie identifica al usuario.
- **OAuth2 / login con Google o Apple**: delegás la autenticación a un proveedor externo.

## 20. Patrones de diseño usados

- **Repository**: para separar el acceso a datos de la lógica de negocio (`IRepository<T>` en este proyecto).
- **Unit of Work**: para agrupar varias operaciones de distintos repositorios en una sola transacción (`IUnitOfWork` acá).
- **Strategy**: para poder cambiar el comportamiento de una regla sin tocar el código que la usa — en este proyecto es justo `IPriceRule`/`IDiscountRule`/`IStockRule` con las implementaciones Base y GanaMax intercambiables por configuración.
- **Adapter**: para envolver algo con una interfaz que no es la que necesito y "traducirla" a la que sí espera mi código, típico cuando consumís una librería externa o una API de terceros que no encaja con tus interfaces internas.
- **Observer**: para que un objeto avise automáticamente a otros cuando cambia su estado, sin que estén acoplados entre sí (por ejemplo eventos en C#, o notificaciones cuando se registra una venta).
