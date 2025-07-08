# 🚗 Fast Parking - Sistema de Gestión de Estacionamiento

## 📋 Descripción
Fast Parking es un sistema completo de gestión de estacionamiento desarrollado en C# con Windows Forms. Permite administrar espacios de estacionamiento con control visual en tiempo real, cálculo automático de tarifas y generación de reportes.

## ✨ Características Principales

### 🎯 **Control de Espacios**
- **15 espacios disponibles** organizados en grilla 3x5 (A1-A5, B1-B5, C1-C5)
- **Visualización en tiempo real** con colores:
  - 🟢 **Verde**: Espacios disponibles
  - 🔴 **Rojo**: Espacios ocupados (muestra placa del vehículo)
- **Control automático de capacidad** - no permite sobrepasar el límite
- **Asignación automática** del primer espacio disponible

### 💰 **Sistema de Tarifas**
- **Auto**: S/ 2.00 por hora
- **Moto**: S/ 1.00 por hora
- **Camión**: S/ 3.00 por hora
- **Cálculo automático** basado en tiempo de permanencia

### 📊 **Estadísticas en Tiempo Real**
- Vehículos activos (X/15)
- Total recaudado
- Promedio de permanencia
- Espacios disponibles

### 📄 **Gestión de Recibos**
- Generación automática de recibos en formato .txt
- Nombres únicos con timestamp
- Información completa: placa, tipo, espacio, horarios, duración, monto

### 📈 **Exportación de Datos**
- Exportar historial completo a Excel (.xlsx)
- Incluye totales y estadísticas
- Compatible con ClosedXML

## 🚀 Tecnologías Utilizadas

- **Framework**: .NET 8.0 Windows
- **UI**: Windows Forms
- **Exportación**: ClosedXML.Excel
- **Lenguaje**: C#

## 🛠️ Requisitos del Sistema

- Windows 10/11
- .NET 8.0 Runtime
- Visual Studio 2022 (para desarrollo)

## 📦 Instalación

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/GhostWarrior1211/FastParking.git
   ```

2. **Abrir el proyecto:**
   ```bash
   cd FastParking
   dotnet restore
   ```

3. **Compilar y ejecutar:**
   ```bash
   dotnet build
   dotnet run
   ```

## 📖 Uso del Sistema

### **Ingresar Vehículo:**
1. Ingrese la placa (6 caracteres alfanuméricos)
2. Seleccione el tipo de vehículo
3. Haga clic en "Ingresar"
4. El sistema asignará automáticamente un espacio disponible

### **Retirar Vehículo:**
1. Seleccione el vehículo de la lista
2. Haga clic en "Retirar"
3. El sistema calculará el monto y liberará el espacio
4. Se generará un recibo automáticamente

### **Exportar Historial:**
- Haga clic en "Exportar Historial" para generar un archivo Excel con todos los registros

## 🏗️ Estructura del Proyecto

```
FastParking/
├── Forms/
│   └── MainForm.cs          # Interfaz principal
├── Models/
│   ├── TipoVehiculo.cs      # Enumeración de tipos
│   └── Vehiculo.cs          # Clase vehículo
├── Services/
│   ├── Estacionamiento.cs   # Lógica principal
│   ├── HistorialSalida.cs   # Gestión historial
│   └── ValidadorPlacas.cs   # Validaciones
├── Tarifas/
│   ├── ICalculadoraTarifa.cs        # Interface tarifas
│   ├── TarifaPorFraccion.cs         # Cálculo por fracción
│   ├── TarifaPorHora.cs             # Cálculo por hora
│   └── TarifaPorTipoVehiculo.cs     # Tarifas por tipo
└── FastParkingCompleto.csproj
```

## 🎨 Interfaz de Usuario

### **Panel Principal:**
- Campos de entrada para placa y tipo de vehículo
- Botones de Ingresar/Retirar
- Lista de vehículos activos

### **Grilla Visual:**
- Representación gráfica de los 15 espacios
- Actualización en tiempo real de colores
- Muestra placa en espacios ocupados

### **Panel de Estadísticas:**
- Información en tiempo real del estacionamiento
- Total recaudado y promedios
- Capacidad actual

### **Panel Informativo:**
- Tarifas del sistema
- Instrucciones de uso
- Información general

## 🔧 Características Técnicas

### **Validaciones:**
- Formato de placa: 6 caracteres alfanuméricos
- Prevención de placas duplicadas
- Control de capacidad máxima

### **Persistencia:**
- Historial en memoria durante la sesión
- Exportación a Excel para persistencia
- Generación de recibos en archivos .txt

### **Arquitectura:**
- Patrón MVC implementado
- Separación de responsabilidades
- Interfaces para extensibilidad

## 📝 Próximas Mejoras

- [ ] Base de datos para persistencia
- [ ] Reportes avanzados
- [ ] Configuración de tarifas
- [ ] Sistema de usuarios
- [ ] API REST
- [ ] Aplicación web

## 👨‍💻 Desarrolladores

**Taipe Quintana Ivan Nelson**

**Rodrigo Manuel Cherre Santillan**

**Jorge Hiroshi Chung Quispe**

**Bryan Alexander Arce Muñoz**
