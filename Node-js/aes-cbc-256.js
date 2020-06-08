
const crypto = require("crypto"); // node.js 기본 모듈 사용

const aesCbc256 = {
  
  /************************************
   * 암호화
   * @param srcText   암호화할 텍스트
   * @param key  32바이트 string or byte Buffer
   * @param iv   16바이트 string or byte Buffer
   * @returns {string}
   */
  encrypt: (srcText, key, iv) => {
    
    try {
      const cipher = crypto.createCipheriv('aes-256-cbc', key, iv);
      const encrypted = cipher.update(srcText, 'utf8', 'base64');
      const final = cipher.final('base64');
      return encrypted + final;
      
    } catch(err) {
      console.error(`FAILED TO ENCRYPT AES-CBC-256: ${srcText}`);
      console.error(err);
      return "";
    }
  },
  
  /*************************************
   * 복호화
   * @param encText  암호화된 Base64 텍스트
   * @param key  32바이트 string or byte Buffer
   * @param iv   16바이트 string or byte Buffer
   * @returns {string}
   */
  decrypt: (encText, key, iv) => {
    try {
      const decipher = crypto.createDecipheriv('aes-256-cbc', key, iv);
      const decrypted = decipher.update(encText, 'base64', 'utf8');
      const final = decipher.final('utf8');
      return decrypted + final;
  
    } catch(err) {
      console.error(`FAILED TO DECRYPT AES-CBC-256: ${encText}`);
      console.error(err);
      return "";
    }
  }
}

module.exports = aesCbc256;