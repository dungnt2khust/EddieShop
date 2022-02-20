import axios from 'axios'

const api = axios.create({
    baseURL: `https://api.cloudflare.com/client/v4/accounts/dc3cafa389c20dd9c6a1e57be03439e2/images/v1/direct_upload`,
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': "*",
        'Access-Control-Allow-Headers': "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With",
        'Access-Control-Allow-Credentials': true,
        'Authorization': process.env.CNDToken
    }
})

export default api;