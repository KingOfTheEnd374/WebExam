import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', () => {
  const jwt = ref<string | null>(null)
  const refreshToken = ref<string | null>(null)

  const isAuthenticated = computed<boolean>(() => jwt.value ? true : false)

  return { jwt, refreshToken, isAuthenticated }
})
