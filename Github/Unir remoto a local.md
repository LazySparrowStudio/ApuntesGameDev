# 🧩 Unir un Proyecto Local con un Repositorio de GitHub Existente

Tienes dos elementos:

1. Un **repositorio local** con tu proyecto completo.
2. Un **repositorio remoto en GitHub**, que ya contiene un archivo `.gitignore`.

Tu objetivo es **unir ambos sin perder datos** ni configuraciones.

---

## 1. Inicializa GIT
git init

## 2. Agregar repositorio remoto
git remote add origin https://github.com/TU_USUARIO/NOMBRE_DEL_REPO.git


## 3. Traer el archivo del remoto
git pull origin main --allow-unrelated-histories


## 4. Agregar, commitear y subir
git add .
git commit -m "Mi primer commit"
git push origin main