<script setup lang="ts">
import { ref, onMounted } from 'vue'

interface TheaterShow {
  id: number
  playTitle: string
  hall: string
  showtime: string
  ticketPrice: number
}

const shows = ref<TheaterShow[]>([])
const loading = ref<boolean>(true)
const error = ref<string | null>(null)

const formatPrice = (price: number) => {
  return new Intl.NumberFormat('de-AT', {
    style: 'currency',
    currency: 'EUR'
  }).format(price)
}

const formatDateTime = (dateString: string) => {
  return new Intl.DateTimeFormat('de-AT', {
    dateStyle: 'medium',
    timeStyle: 'short'
  }).format(new Date(dateString))
}

const fetchShows = async () => {
  try {
    const response = await fetch('http://127.0.0.1:5008/api/shows')
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`)
    }
    shows.value = await response.json()
  } catch (err) {
    error.value = 'Failed to load theater shows from API.'
    console.error(err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchShows()
})
</script>

<template>
  <main class="theater-container">
    <h1>Theaterstücke</h1>

    <div v-if="loading" class="status">Loading showtimes...</div>
    <div v-else-if="error" class="status-error">{{ error }}</div>

    <div v-else class="table-container">
      <table>
        <thead>
          <tr>
            <th>Titel</th>
            <th>Halle</th>
            <th>Datum/Uhrzeit</th>
            <th>Ticketpreis</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="show in shows" :key="show.id">
            <td>{{ show.playTitle }}</td>
            <td>{{ show.hall }}</td>
            <td>{{ formatDateTime(show.showtime) }}</td>
            <td>{{ formatPrice(show.ticketPrice) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>