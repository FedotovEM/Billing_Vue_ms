<template>
    <div>
        <input type="file" ref="fileInput" @change="onFileChange" />
        <button @click="uploadFile">Загрузить</button>
    </div>
</template>

<script>
    import axios from 'axios';
    import authHeader from '../services/auth-header.js';
    import { urls } from '../settings.js';

    let user = JSON.parse(localStorage.getItem('user'));

    export default {
        methods: {
            onFileChange(event) {
                this.selectedFile = event.target.files[0];
            },
            uploadFile() {
                const formData = new FormData();
                formData.append('file', this.selectedFile); 

                axios
                    .post(urls.webapi + "/UploadFile", formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data',
                            'Authorization': 'Bearer ' + user.token
                        },
                    })
                    .then((response) => {
                       
                        console.log(response.data);
                    })
                    .catch((error) => {
                       
                        console.error(error);
                    });
            },
        },
    };

    const handleFileUpload = async (event) => {
      const file = event.target.files[0];
      const reader = new FileReader();

      reader.onload = () => {
        const fileContents = reader.result;
        // Process the file contents or perform any required operations
        console.log(fileContents);
      };

      reader.readAsText(file);
    }
    
   
</script>
