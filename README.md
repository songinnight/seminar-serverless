# 서버리스 - 보안 AES-CBC-256 양방향 암호화

서버와 클라이언트의 key 값은 동일해야하며 보안을 더 강화하기 위해서 IV값을 유저 마다 다르게 설정해도 좋습니다.

## 서버 node.js
[aes-cbc-256.js](Node-js/aes-cbc-256.js) 파일을 원하는 위치에 복사한 후 아래와 같이 사용
> 실 서비스에서는 Key 관련 변수를 서버의 Environment와 같은 곳에 저장할 것을 권장\
> Github 등에 Key 값이 올라가지 않도록 주의.
  ```javascript
  const aesCbc256 = require("./aes-cbc-256");

  const textToEncode = "stage=4&victory=0&killedMob=3&rank=F";

  // ----------------------------------------------------
  // --- 암호 키로 문자열을 사용하는 경우. 클라이언트와 동일해야함 ---
  const keyString = "my secret key4567890123456789012"; // AES-CBC-256의 키 크기 = 256비트 = 32바이트
  const ivString = "1234567890123456"; // IV는 16바이트 고정
  const enc1 = aesCbc256.encrypt(textToEncode, keyString, ivString);
  console.log(`>>> 암호화1: ${enc1}`);
  const dec1 = aesCbc256.decrypt(enc1, keyString, ivString);
  console.log(`복호화1: ${dec1}`);



  // ----------------------------------------------------
  // --- 바이트 배열로 보기 어렵게 만드는 경우. 클라이언트와 동일해야함 ---
  // 키 길이 = 256비트 (=32바이트)
  // 0x00 ~ 0xff 사이의 임의의 값들로 채워 넣으면 됨
  const keyBytes = Buffer.from([
    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xa0,
    0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xfa, 0xf9, 0xf8, 0xf7, 0xf6,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00
  ]);
  // IV 길이는 16바이트 고정
  const ivBytes = Buffer.from([
    0xa1, 0xb1, 0xc1, 0xd1, 0xe1, 0xf1, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00
  ]);
  const enc2 = aesCbc256.encrypt(textToEncode, keyBytes, ivBytes);
  console.log(`>>> 암호화2: ${enc2}`);
  const dec2 = aesCbc256.decrypt(enc2, keyBytes, ivBytes);
  console.log(`복호화2: ${dec2}`);
  ```


## 클라이언트 c#
[AesCbc256.cs](C-Sharp/AesCbc256.cs) 파일을 프로젝트에 포함시킨 후 아래와 같이 사용.
> 유니티의 경우 Mono빌드 대신 IL2CPP빌드를 사용하여 디코딩을 어렵게 만들도록 권장.
  ```csharp
  // 암호화할 텍스트
  string textToEncode = "stage=4&victory=0&killedMob=3&rank=F";
  Debug.Log("암호화할 텍스트 원본: " + textToEncode);


  // ----------------------------------------------
  // --- 암호 키로 문자열을 사용하는 경우. 서버와 동일해야함 ---
  string keyString = "my secret key4567890123456789012";
  string ivString = "1234567890123456"; // 유저 마다 고유 값을 가지고 랜덤하게 생성하는 것을 권장

  string enc1 = AesCbc256.Encrypt(textToEncode, keyString, ivString);
  Debug.Log(">>> 암호화1: " + enc1);
  string dec1 = AesCbc256.Decrypt(enc1, keyString, ivString);
  Debug.Log("복호화1: " + dec1);



  // ----------------------------------------------
  // --- 보기 어렵게 만들기 위해 바이트 배열을 사용하는 경우. 서버와 동일해야함 ---
  // AES-CBC-256 키 길이는 32바이트 (아래 0x00 값들은 00~ff까지 임의로 지정)
  byte[] keyBytes = new byte[] {
    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xa0,
    0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xfa, 0xf9, 0xf8, 0xf7, 0xf6,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00
  };
  // IV 길이는 16바이트 고정 (유저 마다 고유 값을 가지고 랜덤하게 생성하는 것을 권장)
  byte[] ivBytes = new byte[] {
    0xa1, 0xb1, 0xc1, 0xd1, 0xe1, 0xf1, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00
  };

  string enc2 = AesCbc256.Encrypt(textToEncode, keyBytes, ivBytes);
  Debug.Log(">>> 암호화2: " + enc2);
  string dec2 = AesCbc256.Decrypt(enc2, keyBytes, ivBytes);
  Debug.Log("복호화2: " + dec2);
  ```

### key 관련 변수는 IEnumerator 함수 안에 있으면 dotPeek 등의 디컴파일로 소스코드가 곧바로 노출되는 것을 억제할 수 있다.
  ```csharp
  private IEnumerator ObfuscateAgainstDecompiler()
  {
    // ...
    // 암호화 된 문자열
    string encryptedString = "o2Ao5w7YyC5vkJptf7LG0zK8...pPGlgE5GksThlM6i8+Ow";

    // IEnumerator 블록 내에 있는 코드는 리버스 엔지니어링 툴에서 제대로 해석해내지 못한다.
    byte[] encKey = new byte[] {
      0x9f, 0x1b, 0x2a, 0x50, 0x05, 0x5f, 0xb4, 0xfe, 0x89, 0x56,
      0x2a, 0x5f, 0x41, 0xec, 0x98, 0xfe, 0x50, 0xe3, 0xfe, 0x0c,
      0x48, 0x64, 0xeb, 0x2c, 0x3f, 0x79, 0x41, 0xf6, 0x6d, 0xa3,
      0x01, 0x0b
    };

    byte[] encIV = new byte[] {
      0x21, 0x2c, 0xd2, 0x2e, 0xfa, 0xef, 0x6b, 0xb5, 0x3b, 0xcf,
      0x43, 0x8a, 0x71, 0x24, 0xbb, 0x0e
    };

    string decryptedString = AesCbc256.Decrypt(encryptedString, encKey, encIV);

    // 복호화 된 decryptedString를 사용
    // ...


    yield return null;
  }
  ```
