using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
   
    [Header("Product ID")]
    public string product1 = "item_1";
    public string product2 = "item_2";
    public string product3 = "item_3";
    [Header("Cache")]
    private IStoreController storeController; //구매 과정을 제어하는 함수 제공자
    private IExtensionProvider storeExtensionProvider; //여러 플랫폼을 위한 확장 처리 제공자

    void Start()
    {
        InitUnityIAP();
    
    }
    private void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /*구글 플레이 상품들 추가*/
        builder.AddProduct(product1, ProductType.Consumable, new IDs() { { product1, GooglePlay.Name } });
        builder.AddProduct(product2, ProductType.Consumable, new IDs() { { product2, GooglePlay.Name } });
        builder.AddProduct(product3, ProductType.Consumable, new IDs() { { product3, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);

    }

    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId); //상품 정의

        if(product != null && product.availableToPurchase)
        {
            storeController.InitiatePurchase(product);
        }
        else
        {
            UIController.instance.PayFailedWarning();
            Debug.Log("Product missing or purchase cannot be processed.");
        }
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Reset Successful");

        storeController = controller;
        storeExtensionProvider = extensions;

        UIController.instance.price1.text = controller.products.WithID("item_1").metadata.localizedPriceString;
        UIController.instance.price2.text = controller.products.WithID("item_2").metadata.localizedPriceString;
        UIController.instance.price3.text = controller.products.WithID("item_3").metadata.localizedPriceString;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        UIController.instance.PayFailedWarning();

        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        UIController.instance.PayFailedWarning();

        throw new System.NotImplementedException();

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        UIController.instance.PayFailedWarning();

        throw new System.NotImplementedException();

    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log("Purchase Successful");

        if(purchaseEvent.purchasedProduct.definition.id == product1)
        {
            LevelManager.instance.GetGems(150);
        }
        else if(purchaseEvent.purchasedProduct.definition.id == product2)
        {
            LevelManager.instance.GetGems(350);
        }
        else if (purchaseEvent.purchasedProduct.definition.id == product3)
        {
            LevelManager.instance.GetGems(750);
        }


        return PurchaseProcessingResult.Complete;
    }


}
