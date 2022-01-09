import axios from 'axios'

const api = axios.create({
    baseURL: `${process.env.BASE_URL}/api/v1/`,
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': process.env.BASE_URL,
        'Access-Control-Allow-Credentials': true
    }
})

export default api;