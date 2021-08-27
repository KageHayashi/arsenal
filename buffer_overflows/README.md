## Python3 Socket Send Encoding
When using socket.send() in Python3, we must send bytes. If we use .encode('utf-8') or just .encode(), this will encode hex strings as invididual characters instead of an actual hex code. For example, "\xff".encode() would become b'\xc3\xbf'

```python
>>> "\xff".encode()
b'\xc3\xbf'
>>>
```

So, in order to make our payloads work, we must instead encode with 'latin-1':
Note: As far as I can see, encoding with 'latin-1' doesn't mess up other normal strings like "Hello", so we can use it even when our payload has ASCII characters and hex codes like "Hello \xff".

```python
>>> "\xff".encode('latin-1')
b'\xff'
>>> "Hello \xff".encode('latin-1')
b'Hello \xff'
```
