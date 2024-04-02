import Axios from 'axios';

const instance = Axios.create({
    //baseURL: 'http://10.0.22.14:9000',
    //baseURL: 'http://localhost:9000',
    baseURL: process.env.REACT_APP_LOCAL,
    timeout: 1000,
    headers:{
        'Content-Type' : 'application/json'
    }
});

export default instance;