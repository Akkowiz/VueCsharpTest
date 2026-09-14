<script setup lang="ts">
import { ref, onMounted } from 'vue'

interface WeatherForecast {
  date: string
  temperatureC: number
  temperatureF: number
  summary: string | null
}

const forecasts = ref<WeatherForecast[]>([])
const loading = ref<boolean>(true)
const error = ref<string | null>(null)

async function fetchWeather() {
  loading.value = true
  error.value = null

  try {
    const response = await fetch('http://localhost:5008/weatherforecast')
    if (!response.ok) {
      throw new Error(`HTTP error, status: ${response.status}`)
    }
    forecasts.value = await response.json()
  } catch (err: any) {
    error.value = 'Failed to load weather: ' + err.message
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchWeather()
})

</script>

<template>
  <section id="center">
<div class="weather-card">
    <h2>Weather Forecast</h2>

    <p v-if="loading">Loading forecast</p>
    <p v-else-if="error" class="error">{{ error }}</p>

    <div v-else>
      <table border="1">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp (°C)</th>
            <th>Temp (°F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in forecasts" :key="item.date">
            <td>{{ item.date }}</td>
            <td>{{ item.temperatureC }}°C</td>
            <td>{{ item.temperatureF }}°F</td>
            <td>{{ item.summary }}</td>
          </tr>
        </tbody>
      </table>

      <button @click="fetchWeather">Reload Data</button>
    </div>
  </div>
  </section>
  <section id="spacer"></section>
</template>