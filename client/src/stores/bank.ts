import { defineStore } from 'pinia';
import api from '../utils/api';

const demoCredentials = {
  email: 'alice@example.com',
  password: 'Passw0rd!'
};

type Transaction = {
  id: string;
  type: string;
  description: string;
  timestamp: string;
  amount: number;
};

type Account = {
  id: string;
  fullName: string;
  email: string;
  balance: number;
  transactions: Transaction[];
};

export const useBankStore = defineStore('bank', {
  state: () => ({
    accounts: [] as Account[],
    token: localStorage.getItem('token') ?? '',
    audit: [] as string[]
  }),
  getters: {
    transactions: (state) =>
      state.accounts.flatMap((a) => a.transactions).sort((a, b) => (
        new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime()
      ))
  },
  actions: {
    async ensureToken() {
      if (this.token) return;
      const response = await api.post<{ token: string }>('/api/auth/login', demoCredentials);
      this.token = response.data.token;
      localStorage.setItem('token', this.token);
    },
    async fetchAccounts() {
      await this.ensureToken();
      const response = await api.get<Account[]>('/api/accounts');
      this.accounts = response.data;
    },
    async fetchAudit() {
      await this.ensureToken();
      const response = await api.get<string[]>('/api/audit');
      this.audit = response.data;
    }
  }
});
