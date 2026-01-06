<template>
  <div class="layout">
    <header>
      <h1>miniBank</h1>
      <nav>
        <RouterLink to="/">Dashboard</RouterLink>
        <RouterLink to="/audit">Audit</RouterLink>
      </nav>
    </header>
    <main>
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.layout {
  min-height: 100vh;
  background: #f5f7fb;
  color: #1f2937;
}

header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 2rem;
  background: linear-gradient(90deg, #1f6feb, #6dd5ed);
  color: white;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.08);
}

nav {
  display: flex;
  gap: 1rem;
}

nav a {
  color: white;
  text-decoration: none;
  font-weight: 600;
}

main {
  max-width: 1100px;
  margin: 0 auto;
  padding: 2rem;
}
</style>
