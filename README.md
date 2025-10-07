# 👥 User Management API

Una API RESTful completa para la gestión de usuarios desarrollada con **ASP.NET Core 8.0**. Incluye operaciones CRUD completas, validaciones, logging con Serilog y documentación automática con Swagger.

## 🚀 Características

-   ✅ **CRUD completo** para usuarios
-   ✅ **Validaciones automáticas** con Data Annotations
-   ✅ **Documentación API** con Swagger/OpenAPI
-   ✅ **Logging estructurado** con Serilog
-   ✅ **Serialización múltiple** (JSON, XML, Binary)
-   ✅ **Soft Delete** para eliminación segura
-   ✅ **Búsqueda y filtrado** de usuarios
-   ✅ **Estadísticas** en tiempo real
-   ✅ **Manejo de errores** robusto
-   ✅ **Middleware personalizado** para logging

## 🛠️ Tecnologías Utilizadas

-   **Framework**: ASP.NET Core 8.0
-   **Lenguaje**: C# 12
-   **Logging**: Serilog
-   **Documentación**: Swagger/OpenAPI
-   **Validación**: Data Annotations
-   **Serialización**: System.Text.Json, XML, Binary

## 📋 Estructura del Proyecto

```
userManagement/
├── Controllers/
│   ├── UsersController.cs      # CRUD de usuarios
├── Properties/
│   └── launchSettings.json     # Configuración de desarrollo
├── logs/                       # Archivos de log
├── *.http                      # Archivos de prueba HTTP
├── Program.cs                  # Configuración principal
├── appsettings.json           # Configuración de la aplicación
└── userManagement.csproj      # Archivo del proyecto
```

## 🔧 Instalación y Configuración

### Prerrequisitos

-   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Visual Studio Code](https://code.visualstudio.com/) (recomendado)
-   [REST Client Extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) para VS Code

### Pasos de Instalación

1. **Clonar el repositorio:**

    ```bash
    git clone <repository-url>
    cd userManagement
    ```

2. **Restaurar dependencias:**

    ```bash
    dotnet restore
    ```

3. **Compilar el proyecto:**

    ```bash
    dotnet build
    ```

4. **Ejecutar la aplicación:**

    ```bash
    dotnet run
    ```

5. **Acceder a la documentación:**
    - Swagger UI: `https://localhost:5109/swagger`
    - API Base: `https://localhost:5109/api`

## 📖 Documentación de la API

### 🔗 Endpoints Principales

#### **Users Controller (`/api/users`)**

| Método   | Endpoint                          | Descripción                 | Códigos de Estado  |
| -------- | --------------------------------- | --------------------------- | ------------------ |
| `GET`    | `/api/users`                      | Obtener todos los usuarios  | 200                |
| `GET`    | `/api/users/{id}`                 | Obtener usuario por ID      | 200, 404           |
| `GET`    | `/api/users/search?search={term}` | Buscar usuarios             | 200                |
| `GET`    | `/api/users/stats`                | Estadísticas de usuarios    | 200                |
| `POST`   | `/api/users`                      | Crear nuevo usuario         | 201, 400, 409      |
| `PUT`    | `/api/users/{id}`                 | Actualizar usuario completo | 200, 400, 404, 409 |
| `PATCH`  | `/api/users/{id}`                 | Actualizar usuario parcial  | 200, 404           |
| `PATCH`  | `/api/users/{id}/activate`        | Reactivar usuario           | 200, 404           |
| `DELETE` | `/api/users/{id}`                 | Eliminación suave           | 204, 404           |
| `DELETE` | `/api/users/{id}/permanent`       | Eliminación permanente      | 204, 404           |

#### **Person Controller (`/api/person`)**

| Método | Endpoint             | Descripción                        |
| ------ | -------------------- | ---------------------------------- |
| `GET`  | `/api/person/binary` | Deserializar desde archivo binario |
| `GET`  | `/api/person/xml`    | Deserializar desde XML             |
| `GET`  | `/api/person/json`   | Deserializar desde JSON            |
| `POST` | `/api/person`        | Serializar en múltiples formatos   |

### 📝 Modelos de Datos

#### **User Model**

```json
{
	"id": 1,
	"name": "Juan Pérez",
	"email": "juan@email.com",
	"age": 25,
	"createdAt": "2025-10-07T10:30:00Z",
	"isActive": true
}
```

#### **CreateUserRequest**

```json
{
	"name": "Ana García",
	"email": "ana@email.com",
	"age": 28
}
```

#### **UpdateUserRequest**

```json
{
	"name": "Ana García Actualizada",
	"email": "ana.nueva@email.com",
	"age": 29,
	"isActive": true
}
```

### 🔍 Validaciones

-   **Name**: Requerido, 2-100 caracteres
-   **Email**: Requerido, formato válido, único
-   **Age**: 18-100 años
-   **Emails únicos**: No se permiten duplicados

## 🧪 Pruebas

### Archivos de Prueba HTTP

El proyecto incluye archivos `.http` para probar todos los endpoints:

-   **`Users.http`**: Pruebas completas del controlador de usuarios
-   **`Person.http`**: Pruebas de serialización
-   **`Products.http`**: Pruebas del controlador de productos

### Ejecutar Pruebas

1. **Con VS Code + REST Client:**

    - Abre cualquier archivo `.http`
    - Haz clic en "Send Request" sobre cada petición

2. **Con cURL:**

    ```bash
    # Obtener todos los usuarios
    curl -X GET "https://localhost:5109/api/users" -H "accept: application/json"

    # Crear usuario
    curl -X POST "https://localhost:5109/api/users" \
      -H "Content-Type: application/json" \
      -d '{"name":"Test User","email":"test@email.com","age":25}'
    ```

3. **Con Swagger UI:**
    - Ve a `https://localhost:5109/swagger`
    - Prueba los endpoints interactivamente

## 📊 Logging

El proyecto utiliza **Serilog** para logging estructurado:

### Configuración

-   **Console**: Logs en tiempo real
-   **File**: Archivos diarios en `/logs/`
-   **Middleware personalizado**: Tracking de requests

### Ejemplo de Logs

```
[10:30:15 INF] FIRST Handling request: GET /api/users
[10:30:15 INF] SECOND Handling request: GET /api/users
[10:30:15 INF] FIRST Finished handling request.
[10:30:15 INF] SECOND Finished handling request.
```

## 🔧 Configuración

### appsettings.json

```json
{
	"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft.AspNetCore": "Warning"
		}
	},
	"AllowedHosts": "*"
}
```

### Variables de Entorno

-   `ASPNETCORE_ENVIRONMENT`: Development/Production
-   `ASPNETCORE_URLS`: URLs de escucha

## 📈 Características Avanzadas

### Middleware Personalizado

-   **Request/Response Logging**: Seguimiento completo de peticiones
-   **Error Handling**: Manejo global de excepciones
-   **Custom Headers**: Headers personalizados en respuestas

### Serialización Múltiple

-   **JSON**: Por defecto con System.Text.Json
-   **XML**: Con XmlSerializer
-   **Binary**: Con BinaryWriter/BinaryReader

### Soft Delete

-   Los usuarios no se eliminan físicamente
-   Se marcan como `IsActive = false`
-   Posibilidad de reactivación

## 🛡️ Manejo de Errores

| Código  | Descripción   | Ejemplo                         |
| ------- | ------------- | ------------------------------- |
| **200** | Éxito         | Usuario encontrado              |
| **201** | Creado        | Usuario creado exitosamente     |
| **204** | Sin contenido | Usuario eliminado               |
| **400** | Bad Request   | Datos de validación incorrectos |
| **404** | No encontrado | Usuario no existe               |
| **409** | Conflicto     | Email ya registrado             |
| **500** | Error interno | Error del servidor              |

## 🚀 Despliegue

### Desarrollo

```bash
dotnet run --environment Development
```

### Producción

```bash
dotnet publish -c Release
dotnet userManagement.dll
```

### Docker (Opcional)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "userManagement.dll"]
```

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - mira el archivo [LICENSE](LICENSE) para detalles.

## 👨‍💻 Autor

**Pablo Pastorino**

-   GitHub: [@pablopastorino](https://github.com/pablopastorino)

## 📞 Soporte

Si tienes preguntas o necesitas ayuda:

1. **Issues**: Abre un issue en GitHub
2. **Documentación**: Revisa la documentación de Swagger
3. **Logs**: Revisa los archivos de log en `/logs/`

## 🔄 Changelog

### v1.0.0 (2025-10-07)

-   ✅ Implementación inicial del CRUD de usuarios
-   ✅ Integración con Serilog para logging
-   ✅ Documentación con Swagger
-   ✅ Serialización múltiple (JSON, XML, Binary)
-   ✅ Middleware personalizado
-   ✅ Archivos de prueba HTTP
-   ✅ Validaciones automáticas

---

¡Gracias por usar User Management API! 🎉
