# .NET Extra
特定のドメインに寄らない、ベースクラスライブラリの薄い拡張です。

下記の名前空間を少し拡張するイメージで、主にこれらに依存する型を追加しています。

- System
- System.Collections.Generic


## Target Frameworks
- .NET Standard 2.0+
- .NET Standard 1.0+
- .NET Framework 4.5+


## APIs
| Type                           | Description |
| ---                            | ---         |
| Base64Url                      | [base64url](https://tools.ietf.org/html/rfc4648#page-7) のエンコード及びデコードを行うクラス。 |
| Binary                         | `byte` 配列と 16 進文字列の相互変換を行うクラス。 |
| SemanticVersion                | [Semantic Versioning 2.0.0](https://semver.org/) を表現するクラス。 |
| SemanticVersionParser          | 文字列を *Semantic Version 2.0.0* として解釈するパーサー。 |


## Licence
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
