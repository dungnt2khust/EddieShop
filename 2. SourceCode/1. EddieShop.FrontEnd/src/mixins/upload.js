import UploadAPI from "@/api/components/Upload/UploadAPI.js";
const axios = require('axios');

export default {
  methods: {
    /**
     * Tải ảnh lên server
     * @param {*} file
     * @param {*} fileName
     * @param {*} tags
     * @param {*} folder
     * @returns
     * created by ntdung 26.01.2022
     */
    UploadImage(file, fileName,tags =['default'], folder="default") {
      return new Promise((resolve, reject) => {
        UploadAPI.Image() 
          .then(res => {
            if (res.data.Success) {
              var token = res.data.Data;
              axios.post(`https://upload.imagekit.io/api/v1/files/upload`, this.toFormData({
                file,
                publicKey: `public_Rk9tSR2EEq/ahhSc5lLKIUrIYTM=`,
                fileName,
                tags,
                folder,
                ...token
              }))
              .then(res => {
                resolve(res);
              })
              .catch(err => {
                reject(err)
              })
            }
          })
          .catch(err => {
            console.log(err)
          }); 
      });
    },
    toFormData(o) {
      return Object.entries(o).reduce((d,e) => (d.append(...e),d), new FormData())
    }
  }
};
