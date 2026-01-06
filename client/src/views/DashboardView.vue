<template>
  <section class="card">
    <h2>Accounts</h2>
    <div class="grid">
      <div v-for="account in accounts" :key="account.id" class="panel">
        <div class="panel__title">{{ account.fullName }}</div>
        <div class="panel__balance">${{ account.balance.toFixed(2) }}</div>
        <div class="panel__subtitle">{{ account.email }}</div>
      </div>
    </div>
  </section>

  <section class="card">
    <h3>Recent Transactions</h3>
    <table v-if="transactions.length" class="table">
      <thead>
        <tr>
          <th>Type</th>
          <th>Description</th>
          <th>Amount</th>
          <th>When</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="transaction in transactions" :key="transaction.id">
          <td>{{ transaction.type }}</td>
          <td>{{ transaction.description }}</td>
          <td>${{ transaction.amount.toFixed(2) }}</td>
          <td>{{ new Date(transaction.timestamp).toLocaleString() }}</td>
        </tr>
      </tbody>
    </table>
    <p v-else>No activity yet. Use the API to post a transfer.</p>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useBankStore } from '../stores/bank';

const store = useBankStore();
const loading = ref(false);

onMounted(async () => {
  loading.value = true;
  await store.fetchAccounts();
  loading.value = false;
});

const accounts = computed(() => store.accounts);
const transactions = computed(() => store.transactions.slice(0, 5));
</script>

<style scoped>
.card {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.06);
  margin-bottom: 1.5rem;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
  gap: 1rem;
}

.panel {
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  background: #f8fafc;
}

.panel__title {
  font-weight: 700;
  margin-bottom: 0.35rem;
}

.panel__subtitle {
  color: #6b7280;
  font-size: 0.9rem;
}

.panel__balance {
  font-size: 1.6rem;
  font-weight: 800;
  color: #0f766e;
}

.table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 0.5rem;
}

.table th,
.table td {
  text-align: left;
  padding: 0.5rem;
  border-bottom: 1px solid #e5e7eb;
}
</style>
