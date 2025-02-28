# CloudTask HSE NIS

**Консольное .NET-приложение для работы с JSON, контейнеризированное в Docker.**  
_Проект разработан в рамках НИС (Научно-исследовательский семинар)._

---

##Как запустить?

### Локально

1. **Клонируйте репозиторий:**
   ```bash
   git clone https://github.com/yourusername/CloudTask-HSE-NIS.git
   cd CloudTask-HSE-NIS
2. **Соберите Docker-образ:**
   ```bash
   docker build -t cloudtask-hse-nis .
3. **Запустите контейнер:**
   ```bash
   docker run -it cloudtask-hse-nis
