# CloudTask HSE NIS

**Консольное .NET-приложение для работы с JSON, контейнеризированное в Docker.**  
_Проект разработан в рамках НИСа (Научно-исследовательский семинар) "Введение в обланчные технологии"._

---

## Как запустить?

### Локально

1. **Клонируйте репозиторий:**
   ```bash
   git clone https://github.com/Et3rnalDespa1r/CloudTask-HSE-NIS-.git
   cd CloudTask-HSE-NIS-
   cd src
2. **Соберите Docker-образ:**
   ```bash
   docker build -t cloudtask-hse-nis .
3. **Запустите контейнер:**
   ```bash
   docker run -it cloudtask-hse-nis

### Через Docker Hub

1. **Скачайте образ из Docker Hub:**
   ```bash
   docker pull et3rnaldespair/cloudtask-hse-nis:latest

2. **Запустите контейнер:**
   ```bash
   docker run -it et3rnaldespair/cloudtask-hse-nis:latest

3. **Проверьте список запущенных контейнеров:**
   ```bash
   docker ps

